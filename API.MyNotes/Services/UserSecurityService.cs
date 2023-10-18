using API.IServices;
using Entities.Models.Requests;
using Entities.Models.Responses;
using Logic.Exceptions;
using Logic.ILogic;
using Logic.Logic;

namespace API.MyNotes.Services
{
    public class UserSecurityService : IUserSecurityService
    {
        private readonly IUserSecurityLogic _userSecurityLogic;
        public UserSecurityService(IUserSecurityLogic userSecurityLogic)
        {
            _userSecurityLogic = userSecurityLogic;
        }
        public async Task<LoginResponse> GenerateAccessBearerToken(string userName, string userPassword)
        {
            return await _userSecurityLogic.GenerateAuthenticationBearerTokenAsync(userName, userPassword);
        }
        public AuthenticationTokenResponse AuthenticateJWTToken(string authorization, List<string> allowedUserRols)
        {
            ValidateJWTAuthorization(authorization);
            var token = GetJWTFromAuthorization(authorization);
            return _userSecurityLogic.AuthenticateJWTToken(token);
        }
        private void ValidateJWTAuthorization(string authorization)
        {
            if (authorization.Length < 50 || !authorization.Contains('.'))
            {
                throw new BadRequestException(BadRequestExceptionType.InvalidOperation);
            }
        }
        private string GetJWTFromAuthorization(string authorization)
        {
            return authorization.Split(' ')[1];
        }
        public async Task<int> GetUserIdFromIdWeb(Guid idWeb)
        {
            return await _userSecurityLogic.GetUserIdFromIdWeb(idWeb);
        }
        public async Task<string> GenerateRefreshJWT(RefreshTokenRequest request)
        {
            return await _userSecurityLogic.GenerateRefreshJWTFromAccessToken(request.UserIdWeb, request.AccessToken);
        }
    }
}