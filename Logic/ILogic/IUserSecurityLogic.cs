using Entities.Models.Responses;

namespace Logic.ILogic
{
    public interface IUserSecurityLogic
    {
        Task<LoginResponse> GenerateAuthenticationBearerTokenAsync(string userName, string userPassword);
        Task<LoginResponse> AuthenticateAccessBearerTokenAsync(string token);
        AuthenticationTokenResponse AuthenticateJWTToken(string token);
        string HashString(string key);
        Task<int> GetUserIdFromIdWeb(Guid idWeb);
    }
}