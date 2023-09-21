using Entities.Entities;

namespace Entities.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse() { }
        public LoginResponse(UserItem user, string accesToken) {
            UserIdWeb = user.IdWeb;
            UserName = user.Name;
            AccessToken = accesToken;
            IdRol = user.RolId;
        }
        public Guid UserIdWeb { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public int IdRol { get; set; }
    }
    public class ValidateTokenResponse
    {
        public ValidateTokenResponse() { }
        public ValidateTokenResponse(UserItem user, bool validated)
        {
            UserId = user.Id;
            UserIdWeb = user.IdWeb;
            UserName = user.Name;
            UserIdRol = user.RolId;
            Validated = validated;
        }
        public int UserId { get; set; }
        public Guid UserIdWeb { get; set; }
        public string UserName { get; set; }
        public int UserIdRol { get; set; }
        public bool Validated { get; set; }
    }
}