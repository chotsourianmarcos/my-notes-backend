using Entities.Entities;
using Entities.Enums;

namespace Entities.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse() { }
        public LoginResponse(UserItem user, string accessToken, string refreshToken) {
            UserIdWeb = user.IdWeb;
            UserName = user.Name;
            UserRol = ((UserRolEnum)user.IdRol).ToString();
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        public string UserName { get; set; }
        public Guid UserIdWeb { get; set; }
        public string UserRol { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class AuthenticationTokenResponse
    {
        public AuthenticationTokenResponse() { }
        public AuthenticationTokenResponse(bool validated, string refreshedToken, AuthenticationUserData userData)
        {
            RefreshedToken = refreshedToken;
            UserData = userData;
            Validated = validated;
        }
        public string RefreshedToken { get; set; }
        public AuthenticationUserData UserData { get;set;}
        public bool Validated { get; set; }
        
    }
    public class AuthenticationUserData
    {
        public AuthenticationUserData(string userName, string userIdWebString, string userRolName) 
        {
            UserName = userName;
            UserIdWeb = Guid.Parse(userIdWebString);
            UserIdRol = (int)Enum.Parse(typeof(UserRolEnum), userRolName);
        }
        public string UserName { get; set; }
        public Guid UserIdWeb { get; set; }
        public int UserIdRol { get; set; }
    }
}