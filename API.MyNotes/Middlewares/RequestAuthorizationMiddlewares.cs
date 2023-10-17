using API.Attributes;
using API.IServices;
using Logic.Exceptions;
using Logic.Logic;

namespace API.Middlewares
{
    public class RequestAuthorizationMiddleware : UserSessionLogic
    {
        private readonly RequestDelegate _next;
        public RequestAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, IUserSecurityService userSecurityService)
        {
            SetCurrentUserId(0);
            SetCurrentUserName("");
            SetCurrentUserIdWeb(Guid.Empty);
            SetCurrentUserIdRol(0);

            if (httpContext.Request.Method == "OPTIONS")
            {
                await _next(httpContext);
            }else
            {
                EndpointAuthorizeAttribute authorization = new EndpointAuthorizeAttribute(httpContext);
                if (authorization.Values.AllowsAnonymous)
                {
                    await _next(httpContext);
                }
                else
                {
                    var validationResponse = userSecurityService.AuthenticateJWTToken(httpContext.Request.Headers.Authorization.ToString(),
                                                 authorization.Values.AllowedUserRols);

                    if (!validationResponse.Validated)
                    {
                        throw new AuthenticationException(AuthenticationExceptionType.WrongCredentials);
                    }

                    SetCurrentUserId(await userSecurityService.GetUserIdFromIdWeb(validationResponse.UserData.UserIdWeb));
                    SetCurrentUserName(validationResponse.UserData.UserName);
                    SetCurrentUserIdWeb(validationResponse.UserData.UserIdWeb);
                    SetCurrentUserIdRol(validationResponse.UserData.UserIdRol);

                    await _next(httpContext);
                }
            }
        }
    }
    public static class RequestAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestAuthorizationMiddleware>();
        }
    }
}