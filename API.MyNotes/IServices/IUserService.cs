using Entities.Models.Requests;

namespace API.IServices
{
    public interface IUserService
    {
        Task UserRegister(UserRegisterRequest registerRequest);
    }
}