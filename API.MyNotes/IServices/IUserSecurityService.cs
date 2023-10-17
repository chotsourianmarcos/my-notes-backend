using Entities.Models.Responses;

namespace API.IServices
{
    public interface IUserSecurityService
    {
        Task<LoginResponse> GenerateAccessBearerToken(string userName, string userPassword);
        AuthenticationTokenResponse AuthenticateJWTToken(string authorization, List<string> allowedUserRols);
        Task<int> GetUserIdFromIdWeb(Guid idWeb);
    }
}