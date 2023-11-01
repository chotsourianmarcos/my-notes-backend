using API.Attributes;
using API.IServices;
using Entities.Enums;
using Logic.Exceptions;
using Logic.Logic;

namespace API.Middlewares
{
    public class RequestAuthorizationMiddleware : UserSessionSetLogic
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

                    SetCurrentUserName(validationResponse.UserData.UserName);
                    SetCurrentUserIdWeb(Guid.Parse(validationResponse.UserData.UserIdWeb));
                    SetCurrentUserIdRol((int)Enum.Parse(typeof(UserRolEnum), validationResponse.UserData.UserRolName));

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