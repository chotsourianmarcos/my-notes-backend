using Entities.Entities;
using Entities.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Entities.Models.DataModels
{
    public class JWTData
    {
        public JWTData(UserItem user)
        {
            UserName = user.Name;
            UserIdWeb = user.IdWeb.ToString();
            UserRolName = ((UserRolEnum)user.IdRol).ToString();
        }
        public JWTData(string userName, string userIdWeb, string userRolName)
        {
            UserName = userName;
            UserIdWeb = userIdWeb;
            UserRolName = userRolName;
        }
        public JWTData(string jwtoken)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(jwtoken);

            var claims = decodedValue.Claims;
            this.UserName = decodedValue.Claims.First(c => c.Type == "userName").Value;
            this.UserIdWeb = decodedValue.Claims.First(c => c.Type == "userIdWeb").Value;
            this.UserRolName = decodedValue.Claims.First(c => c.Type == "userRol").Value;

            this.ValidTo = decodedValue.ValidTo;
        }
        public string UserName { get; set; }
        public string UserIdWeb { get; set; }
        public string UserRolName { get; set; }
        public DateTime ValidTo { get; set; }
    }
}