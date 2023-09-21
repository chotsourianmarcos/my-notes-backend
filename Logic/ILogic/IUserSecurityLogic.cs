using Entities.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ILogic
{
    public interface IUserSecurityLogic
    {
        Task<LoginResponse> GenerateAuthorizationTokenAsync(string userName, string userPassword);
        Task<ValidateTokenResponse> ValidateUserTokenAsync(string userNameEncrypted, string token, List<string> allowedUserRols);
        string HashString(string key);
    }
}
