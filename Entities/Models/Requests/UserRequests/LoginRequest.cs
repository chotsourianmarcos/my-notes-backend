namespace Entities.Models.Requests.UserRequests
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }
}
