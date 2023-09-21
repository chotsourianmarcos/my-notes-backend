using Data;
using Entities.Models.Responses;
using Logic.Exceptions;
using Logic.ILogic;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Logic.Logic
{
    public class UserSecurityLogic : IUserSecurityLogic
    {
        private readonly ServiceContext _serviceContext;
        public UserSecurityLogic(ServiceContext serviceContext)
        {
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
        public async Task<LoginResponse> GenerateAuthorizationTokenAsync(string userName, string userPassword)
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

            var secureRandomString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var securedToken = HashString(secureRandomString);
            user.HashedToken = securedToken;
            user.TokenExpireDate = DateTime.Now.AddMinutes(10);

            await _serviceContext.SaveChangesAsync();

            return new LoginResponse(user, secureRandomString);
        }
        public async Task<ValidateTokenResponse> ValidateUserTokenAsync(string userName, string token, List<string> authorizedRols)
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

            var userRol = await _serviceContext.UserRols.Where(r => r.Id == user.RolId).FirstOrDefaultAsync();

            if (userRol == null || !userRol.IsActive)
            {
                throw new BadRequestException(BadRequestExceptionType.InvalidOperation);
            }

            bool authorizedRol = authorizedRols.Any(r => r.Equals(userRol.Name));

            if (!authorizedRol)
            {
                throw new AuthenticationException(AuthenticationExceptionType.RolNotAuthorized);
            }

            if (!VerifyHashedKey(token, user.HashedToken))
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }

            if (DateTime.Now > user.TokenExpireDate)
            {
                throw new AuthenticationException(AuthenticationExceptionType.ExpiredToken);
            }

            user.TokenExpireDate = DateTime.Now.AddMinutes(10);

            await _serviceContext.SaveChangesAsync();

            return new ValidateTokenResponse(user, true);
        }
    }
}