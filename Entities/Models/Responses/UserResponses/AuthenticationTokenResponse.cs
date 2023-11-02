using Entities.Models.DataModels;

namespace Entities.Models.Responses.UserResponses
{
    public class AuthenticationTokenResponse
    {
        public AuthenticationTokenResponse() 
        {
            UserData = new ResponseUserData();
        }
        public AuthenticationTokenResponse(bool validated, string accessToken, ResponseUserData userData)
        {
            Validated = validated;
            AccessToken = accessToken;
            UserData = userData;
        }
        public bool Validated { get; set; }
        public string AccessToken { get; set; }
        public ResponseUserData UserData { get; set; }
    }
}