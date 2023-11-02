using Data;
using Entities.Entities;
using Entities.Models.DataModels;
using Entities.Models.Responses.UserResponses;
using Logic.Exceptions;
using Logic.ILogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Logic.Logic
{
    public class UserSecurityLogic : IUserSecurityLogic
    {
        private readonly IConfiguration _config;
        private readonly ServiceContext _serviceContext;
        public UserSecurityLogic(IConfiguration config, ServiceContext serviceContext)
        {
            _config = config;
            _serviceContext = serviceContext;
        }
        public string HashString(string key)
        {
            return BCrypt.Net.BCrypt.HashPassword(key);
        }
        private bool VerifyHashedKey(string key, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(key, hash);
        }
        private string SymmetricEncrypt(string stringToEncrypt)
        {
            try
            {
                using (var aes = new AesCryptoServiceProvider()
                {
                    Key = Encoding.UTF8.GetBytes(_config["SymmetricEncryption:Key"]),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                })
                {
                    var input = Encoding.UTF8.GetBytes(stringToEncrypt);
                    aes.GenerateIV();
                    var iv = aes.IV;
                    using (var encrypter = aes.CreateEncryptor(aes.Key, iv))
                    using (var cipherStream = new MemoryStream())
                    {
                        using (var tCryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                        using (var tBinaryWriter = new BinaryWriter(tCryptoStream))
                        {
                            cipherStream.Write(iv);
                            tBinaryWriter.Write(input);
                            tCryptoStream.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(cipherStream.ToArray());
                    }
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
        private string SymmetricDecrypt(string cipherString)
        {
            byte[] input = Convert.FromBase64String(cipherString);
            var aes = new AesCryptoServiceProvider()
            {
                Key = Encoding.UTF8.GetBytes(_config["SymmetricEncryption:Key"]),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            var iv = new byte[16];
            Array.Copy(input, 0, iv, 0, iv.Length);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, iv), CryptoStreamMode.Write))
                using (var binaryWriter = new BinaryWriter(cs))
                {
                    binaryWriter.Write(
                        input,
                        iv.Length,
                        input.Length - iv.Length
                    );
                }

                return Encoding.Default.GetString(ms.ToArray());
            }
        }
        private async Task<UserItem> BasicUserAuthentication(string userName, string userPassword)
        {
            var user = await _serviceContext.Users.Where(u => u.Name == userName).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new AuthenticationException(AuthenticationExceptionType.NotExistingUser);
            }

            if (user.IsActive == false)
            {
                throw new AuthenticationException(AuthenticationExceptionType.BlockedAccount);
            }

            if (!VerifyHashedKey(userPassword, user.HashedPassword))
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }

            return user;
        }
        public async Task<LoginResponse> GenerateRefreshBearerToken(string userName, string userPassword)
        {
            var user = await BasicUserAuthentication(userName, userPassword);

            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var accessToken = GenerateJWTAuthenticationToken(new JWTData(user));

            user.HashedRefreshToken = HashString(refreshToken);

            await _serviceContext.SaveChangesAsync();

            return new LoginResponse(user, SymmetricEncrypt(userName) + ":" + refreshToken, accessToken);
        }
        public async Task<string> GenerateAccessJWT(Guid userIdWeb, string token)
        {
            var userName = "";
            try
            {
                userName = SymmetricDecrypt(token.Split(':')[0]);
            }catch(ArgumentException)
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }
            
            var user = await _serviceContext.Users.Where(u => u.Name.Equals(userName)).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new AuthenticationException(AuthenticationExceptionType.NotExistingUser);
            }

            if (user.IsActive == false)
            {
                throw new AuthenticationException(AuthenticationExceptionType.BlockedAccount);
            }

            if (!user.IdWeb.Equals(userIdWeb))
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }

            if (!VerifyHashedKey(token.Split(':')[1], user.HashedRefreshToken))
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }

            var jwtClaims = new JWTData(user);

            return GenerateJWTAuthenticationToken(jwtClaims);
        }
        private string GenerateJWTAuthenticationToken(JWTData jwtata)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("userName", jwtata.UserName),
                new Claim("userIdWeb", jwtata.UserIdWeb),
                new Claim("userRol", jwtata.UserRolName)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:ExpiracyAfter"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public AuthenticationTokenResponse AuthenticateJWTToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetJWTValidationParameters();
            SecurityToken validatedToken;
            var date = DateTime.Now;

            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch(SecurityTokenExpiredException)
            {
                throw new AuthenticationException(AuthenticationExceptionType.ExpiredToken);
            }

            return RefreshJWTAuthenticationToken(token);
        }
        private AuthenticationTokenResponse RefreshJWTAuthenticationToken(string oldToken)
        {
            var tokenData = new JWTData(oldToken);
            var userData = new ResponseUserData(tokenData.UserName, tokenData.UserIdWeb, tokenData.UserRolName);

            if (tokenData.ValidTo < DateTime.Now.AddMinutes(-Convert.ToInt32(_config["Jwt:RefreshOn"]))){
                var refreshedToken = GenerateJWTAuthenticationToken(tokenData);
                return new AuthenticationTokenResponse(true, refreshedToken, userData);
            }
            else
            {
                return new AuthenticationTokenResponse(true, oldToken, userData);
            }
        }
        private TokenValidationParameters GetJWTValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ClockSkew = new TimeSpan(Convert.ToInt32(_config["Jwt:ClockSkew"])),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
            };
        }
        public async Task<int> GetUserIdFromIdWeb(Guid idWeb)
        {
            var user = await _serviceContext.Users.Where(u => u.IdWeb == idWeb).FirstOrDefaultAsync();
            
            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return 0;
            }
        }
    }
}