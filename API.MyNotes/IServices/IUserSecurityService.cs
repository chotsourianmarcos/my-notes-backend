using Entities.Models.Requests.UserRequests;
using Entities.Models.Responses.UserResponses;

namespace API.IServices
{
    public interface IUserSecurityService
    {
        Task<LoginResponse> GenerateRefreshBearerToken(string userName, string userPassword);
        AuthenticationTokenResponse AuthenticateJWTToken(string authorization, List<string> allowedUserRols);
        Task<string> GenerateAccessJWT(AccessTokenRequest accessToken);
    }
}