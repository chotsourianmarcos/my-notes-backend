using Data;
using Entities.Entities;
using Entities.Enums;
using Entities.Models.Responses;
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
        private string SymmetricEncrypt(string data)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes(_config["SymmetricEncryption:RandomInitializer"]);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_config["SymmetricEncryption:Key"]);
                aes.IV = initializationVector;
                var symmetricEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream,
                        symmetricEncryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream as Stream))
                        {
                            streamWriter.Write(data);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }
        private string SymmetricDecrypt(string cipherText)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("SymmetricEncryption:RandomInitializer");
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_config["SymmetricEncryption:Key"]);
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream,
                        decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream as Stream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
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
        public async Task<LoginResponse> GenerateAuthenticationBearerTokenAsync(string userName, string userPassword)
        {
            try
            {
                var user = await BasicUserAuthentication(userName, userPassword);

                var accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                var refreshToken = GenerateJWTAuthenticationToken(new JWTData(user));

                user.HashedAccessToken = SymmetricEncrypt(userName) + ":" + HashString(accessToken);
                user.HashedRefreshToken = HashString(refreshToken);

                await _serviceContext.SaveChangesAsync();

                return new LoginResponse(user, SymmetricEncrypt(userName) + ":" + accessToken, refreshToken);
            }catch(Exception ex)
            {
                var asd = 0;
                throw ex;
            }
            
        }
        public async Task<LoginResponse> AuthenticateAccessBearerTokenAsync(string token)
        {
            var userName = "";
            try
            {
                userName = SymmetricDecrypt(token.Split(':')[0]);
            }catch(ArgumentException)
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }
            
            var user = await _serviceContext.Users.Where(u => u.Name == userName).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new AuthenticationException(AuthenticationExceptionType.NotExistingUser);
            }

            if (user.IsActive == false)
            {
                throw new AuthenticationException(AuthenticationExceptionType.BlockedAccount);
            }

            if (!VerifyHashedKey(token, user.HashedAccessToken.Split(':')[1]))
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }
            await _serviceContext.SaveChangesAsync();

            var jwtClaims = new JWTData(user);

            return new LoginResponse(user, token, GenerateJWTAuthenticationToken(jwtClaims));
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
            var userData = new AuthenticationUserData(tokenData.UserName, tokenData.UserIdWeb, tokenData.UserRolName);
            if (tokenData.ValidTo < DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:RefreshOn"]))){
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
    public class JWTData
    {
        public JWTData(UserItem user)
        {
            UserName = user.Name;
            UserIdWeb = user.IdWeb.ToString();
            UserRolName = ((UserRolEnum)user.IdRol).ToString();
        }
        public JWTData(string userName, string userIdWeb, string userRolName)
        {
            UserName = userName;
            UserIdWeb = userIdWeb;
            UserRolName = userRolName;
        }
        public JWTData(string jwtoken)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(jwtoken);

            var claims = decodedValue.Claims;
            this.UserName = decodedValue.Claims.First(c => c.Type == "userName").Value;
            this.UserIdWeb = decodedValue.Claims.First(c => c.Type == "userIdWeb").Value;
            this.UserRolName = decodedValue.Claims.First(c => c.Type == "userRol").Value;

            this.ValidTo = decodedValue.ValidTo;
        }
        public string UserName { get; set; }
        public string UserIdWeb { get; set; }
        public string UserRolName { get; set; }
        public DateTime ValidTo { get; set; }
    }
}