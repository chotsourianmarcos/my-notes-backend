namespace Entities.Models.Requests.UserRequests
{
    public class AccessTokenRequest
    {
        public string AccessToken { get; set; } = "";
        public Guid UserIdWeb { get; set; }
    }
}