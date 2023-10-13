using API.IServices;
using Entities.Models.Responses;
using Logic.Exceptions;
using Logic.ILogic;

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
            //ValidateBearerAuthorization(authorization);
            //var userName = GetUserNameFromBearerAuthorization(authorization);
            //var token = GetTokenFromBearerAuthorization(authorization, userName);
            return _userSecurityLogic.AuthenticateJWTToken(authorization);
        }
        //private void ValidateBearerAuthorization(string authorization)
        //{
        //    if (authorization.Length < 50 || !authorization.Contains(':'))
        //    {
        //        throw new BadRequestException(BadRequestExceptionType.InvalidOperation);
        //    }
        //}
        //private string GetUserNameFromBearerAuthorization(string authorization)
        //{
        //    var indexToSplit = authorization.IndexOf(':');
        //    return authorization.Substring(7, indexToSplit - 7);
        //}
        //private string GetTokenFromBearerAuthorization(string authorization, string userName)
        //{
        //    return authorization.Split(':')[1];
        //}
    }
}