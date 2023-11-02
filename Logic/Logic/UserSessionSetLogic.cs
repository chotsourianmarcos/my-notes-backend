namespace Logic.Logic
{
    public class UserSessionSetLogic
    {
        internal protected static int CurrentUserId = 0;
        internal protected static string CurrentUserName = "";
        internal protected static Guid CurrentUserIdWeb = Guid.Empty;
        internal protected static int CurrentUserIdRol = 0;

        protected void SetCurrentUserName(string value)
        {
            CurrentUserName = value;
        }
        protected void SetCurrentUserId(int value)
        {
            CurrentUserId = value;
        }
        protected void SetCurrentUserIdWeb(Guid value)
        {
            CurrentUserIdWeb = value;
        }
        protected void SetCurrentUserIdRol(int value)
        {
            CurrentUserIdRol = value;
        }
    }
}