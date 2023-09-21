using Entities.Entities;

namespace Logic.ILogic
{
    public interface IUserLogic
    {
        Task InsertUserAsync(UserItem userItem);
    }
}
