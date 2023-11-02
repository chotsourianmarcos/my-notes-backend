using API.IServices;
using Entities.Enums;
using Entities.Models.Requests.UserRequests;
using Logic.ILogic;

namespace API.MyNotes.Services
{
    public class UserService : IUserService
    {
        private readonly IUserLogic _userLogic;
        private readonly IUserSecurityLogic _userSecurityLogic;
        public UserService(IUserLogic userLogic, IUserSecurityLogic userSecurityLogic)
        {
            _userLogic = userLogic;
            _userSecurityLogic = userSecurityLogic;
        }
        public async Task UserRegister(UserRegisterRequest registerRequest)
        {
            var hashedPassword = _userSecurityLogic.HashString(registerRequest.UserPassword);
            var newUserItem = registerRequest.ToUserItem(hashedPassword);
            newUserItem.IdRol = (int)UserRolEnum.Usuario;
            
            await _userLogic.InsertUserAsync(newUserItem);
        }
    }
}