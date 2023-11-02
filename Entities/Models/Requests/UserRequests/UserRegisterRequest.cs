using Entities.Entities;

namespace Entities.Models.Requests.UserRequests
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string UserPassword { get; set; } = "";
        public UserItem ToUserItem(string hashedPassword)
        {
            var newUserItem = new UserItem();

            newUserItem.Name = UserName;
            newUserItem.Email = UserEmail;
            newUserItem.HashedPassword = hashedPassword;
            newUserItem.InsertDate = DateTime.Now;
            newUserItem.IsActive = true;

            return newUserItem;
        }
    }
}