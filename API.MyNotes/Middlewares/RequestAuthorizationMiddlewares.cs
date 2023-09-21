using API.Attributes;
using API.IServices;
using Logic.Exceptions;
using Logic.Logic;

namespace API.Middlewares
{
    public class RequestAuthorizationMiddleware : UserSessionLogic
    {
        private readonly IUserSecurityService _userSecurityService;
        public RequestAuthorizationMiddleware(IUserSecurityService userSecurityService)
        {
            _userSecurityService = userSecurityService;
        }
        public async Task ValidateRequestAutorizathion(HttpContext context)
        {
            SetCurrentUserId(0);
            SetCurrentUserIdWeb(Guid.Empty);
            SetCurrentUserIdRol(0);
            if (context.Request.Method == "OPTIONS")
            {
                return;
            }

            EndpointAuthorizeAttribute authorization = new EndpointAuthorizeAttribute(context);

            if (authorization.Values.AllowsAnonymous)
            {
                return;
            }

            var validationResponse = await _userSecurityService.ValidateUserToken(context.Request.Headers.Authorization.ToString(),
                                                 authorization.Values.AllowedUserRols);

            if (!validationResponse.Validated)
            {
                throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
            }
            SetCurrentUserId(validationResponse.UserId);
            SetCurrentUserIdWeb(validationResponse.UserIdWeb);
            SetCurrentUserIdRol(validationResponse.UserIdRol);
        }
    }
}