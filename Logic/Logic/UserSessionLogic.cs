namespace Logic.Logic
{
    public class UserSessionLogic
    {
        private static int CurrentUserId = 0;
        private static string CurrentUserName = "";
        private static Guid CurrentUserIdWeb = Guid.Empty;
        private static int CurrentUserIdRol = 0;
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
        public static string GetCurrentUserName()
        {
            return CurrentUserName;
        }
        public static int GetCurrentUserId()
        {
            return CurrentUserId;
        }
        public static Guid GetCurrentUserIdWeb()
        {
            return CurrentUserIdWeb;
        }
       
        public static int GetCurrentUserIdRol()
        {
            return CurrentUserIdRol;
        }
    }
}