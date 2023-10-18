using Entities.Models.Responses;

namespace Logic.ILogic
{
    public interface IUserSecurityLogic
    {
        Task<LoginResponse> GenerateAuthenticationBearerTokenAsync(string userName, string userPassword);
        AuthenticationTokenResponse AuthenticateJWTToken(string token);
        Task<string> GenerateRefreshJWTFromAccessToken(Guid userIdWeb, string accessToken);
        string HashString(string key);
        Task<int> GetUserIdFromIdWeb(Guid idWeb);
    }
}