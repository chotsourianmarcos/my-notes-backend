using Entities.Models.Responses;

namespace API.IServices
{
    public interface IUserSecurityService
    {
        Task<LoginResponse> GenerateAuthorizationToken(string userName, string userPassword);
        Task<ValidateTokenResponse> ValidateUserToken(string authorization, List<string> allowedUserRols);
    }
}