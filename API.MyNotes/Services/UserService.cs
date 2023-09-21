using API.IServices;
using Entities.Enums;
using Entities.Models.Requests;
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
            var newUserItem = registerRequest.ToUserItem();
            newUserItem.RolId = (int)UserRolEnums.Usuario;
            newUserItem.HashedPassword = _userSecurityLogic.HashString(registerRequest.userPassword);

            await _userLogic.InsertUserAsync(newUserItem);
        }
    }
}