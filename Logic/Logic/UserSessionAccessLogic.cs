using Data;
using Logic.ILogic;
using Microsoft.EntityFrameworkCore;

namespace Logic.Logic
{
    public class UserSessionAccessLogic : UserSessionSetLogic, IUserSessionAccessLogic
    {
        private readonly ServiceContext _serviceContext;
        public UserSessionAccessLogic(ServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
        }

        public string GetCurrentUserName()
        {
            return CurrentUserName;
        }
        public async Task<int> GetCurrentUserId()
        {
            if (CurrentUserId == 0 && _serviceContext != null)
            {
                var user = await _serviceContext.Users.Where(u => u.IdWeb == CurrentUserIdWeb).FirstOrDefaultAsync();
                CurrentUserId = user.Id;
            }
            return CurrentUserId;
        }
        public Guid GetCurrentUserIdWeb()
        {
            return CurrentUserIdWeb;
        }
        public int GetCurrentUserIdRol()
        {
            return CurrentUserIdRol;
        }
    }
}