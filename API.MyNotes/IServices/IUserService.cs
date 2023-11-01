using Entities.Models.Requests.UserRequests;

namespace API.IServices
{
    public interface IUserService
    {
        Task UserRegister(UserRegisterRequest registerRequest);
    }
}