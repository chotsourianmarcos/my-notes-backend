namespace Logic.ILogic
{
    public interface IUserSessionAccessLogic
    {
        string GetCurrentUserName();
        Task<int> GetCurrentUserId();
        Guid GetCurrentUserIdWeb();
        int GetCurrentUserIdRol();
    }
}