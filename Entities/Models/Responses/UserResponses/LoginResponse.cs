using Entities.Entities;
using Entities.Enums;
using Entities.Models.DataModels;

namespace Entities.Models.Responses.UserResponses
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            AuthenticationUserData = new ResponseUserData();
        }
        public LoginResponse(UserItem user, string refreshToken, string accessToken)
        {
            AuthenticationUserData = new ResponseUserData();
            AuthenticationUserData.UserIdWeb = user.IdWeb.ToString();
            AuthenticationUserData.UserName = user.Name;
            AuthenticationUserData.UserRolName = ((UserRolEnum)user.IdRol).ToString();
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        public ResponseUserData AuthenticationUserData { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}