using Entities.Entities;

namespace Entities.Models.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }
    public class UserRegisterRequest
    {
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public UserItem ToUserItem()
        {
            var newUserItem = new UserItem();

            newUserItem.IdWeb = Guid.NewGuid();
            newUserItem.Name = userName;
            newUserItem.Email = userEmail;
            newUserItem.InsertDate = DateTime.Now;
            newUserItem.IsActive = true;

            return newUserItem;
        }
    }
}