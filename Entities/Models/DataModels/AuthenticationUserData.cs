namespace Entities.Models.DataModels
{
    public class ResponseUserData
    {
        public ResponseUserData() { }
        public ResponseUserData(string userName, string userIdWeb, string userRolName)
        {
            UserName = userName;
            UserIdWeb = userIdWeb;
            UserRolName = userRolName;
        }
        public string UserName { get; set; }
        public string UserIdWeb { get; set; }
        public string UserRolName { get; set; }
    }
}
