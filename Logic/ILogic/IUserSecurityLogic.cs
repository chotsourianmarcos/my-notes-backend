
using Entities.Models.Responses.UserResponses;

namespace Logic.ILogic
{
    public interface IUserSecurityLogic
    {
        Task<LoginResponse> GenerateRefreshBearerToken(string userName, string userPassword);
        AuthenticationTokenResponse AuthenticateJWTToken(string token);
        Task<string> GenerateAccessJWT(Guid userIdWeb, string accessToken);
        string HashString(string key);
        Task<int> GetUserIdFromIdWeb(Guid idWeb);
    }
}