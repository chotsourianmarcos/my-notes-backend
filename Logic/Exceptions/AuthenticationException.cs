namespace Logic.Exceptions
{
    public enum AuthenticationExceptionType
    {
       NotExistingUser = 1,
       WrongCredentials = 2,
       ExpiredToken = 3,
       BlockedAccount = 4,
       RolNotAuthorized = 5
    }
    public class BadAuthenticationResp : HttpResponseMessage
    {
        public BadAuthenticationResp(AuthenticationExceptionType type, string customMessage = "", string errorCode = "")
        {
            switch ((int)type)
            {
                case (int)AuthenticationExceptionType.NotExistingUser:
                    this.StatusCode = System.Net.HttpStatusCode.NotFound;
                    this.Content = new StringContent("It was not found an user for the given credentials.");
                    this.ReasonPhrase = "Not existing user.";
                    break;
                case (int)AuthenticationExceptionType.WrongCredentials:
                    this.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    this.Content = new StringContent("Credentials are not correct for the given user.");
                    this.ReasonPhrase = "Wrong credentials.";
                    break;
                case (int)AuthenticationExceptionType.ExpiredToken:
                    this.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    this.Content = new StringContent("User session expired. Please login again.");
                    this.ReasonPhrase = "Expired token.";
                    break;
                case (int)AuthenticationExceptionType.BlockedAccount:
                    this.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    this.Content = new StringContent("User has been blocked or deactivated. Please contact the support department.");
                    this.ReasonPhrase = "User blocked or not active.";
                    break;
                case (int)AuthenticationExceptionType.RolNotAuthorized:
                    this.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    this.Content = new StringContent("User does not have permission for the requested operation.");
                    this.ReasonPhrase = "User rol unauthorized.";
                    break;
                case '_':
                    break;
            }

            if (!String.IsNullOrEmpty(customMessage))
            {
                this.Content = new StringContent(customMessage);
            }
            if (!String.IsNullOrEmpty(errorCode))
            {
                this.ReasonPhrase = errorCode + ":" + this.ReasonPhrase;
            }
        }
    }
    public class AuthenticationException : LogicControlledException
    {
        public AuthenticationException(AuthenticationExceptionType type, string customMessage = "", string errorCode = "") : base(new BadAuthenticationResp(type, customMessage, errorCode))
        {

        }
    }
}