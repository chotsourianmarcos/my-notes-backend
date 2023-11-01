using Entities.Entities;

namespace Entities.Models.Requests.UserRequests
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public UserItem ToUserItem()
        {
            var newUserItem = new UserItem();

            newUserItem.IdWeb = Guid.NewGuid();
            newUserItem.Name = UserName;
            newUserItem.Email = UserEmail;
            newUserItem.InsertDate = DateTime.Now;
            newUserItem.IsActive = true;

            return newUserItem;
        }
    }
}
