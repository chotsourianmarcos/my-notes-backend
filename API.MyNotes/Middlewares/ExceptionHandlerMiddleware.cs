using API.IServices;
using Logic.Exceptions;
using System.Text.Json;

namespace API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, IUserSecurityService userSecurityService)
        {
            try
            {
                await _next(httpContext);
            }
            catch(ControlledException ex)
            {
                await HandleControlledExceptionAsync(httpContext, ex);
            }
            
        }
        private async Task HandleControlledExceptionAsync(HttpContext context, ControlledException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.HttpResponseMessage.StatusCode;
            var message = await ex.HttpResponseMessage.Content.ReadAsStringAsync();
            var reasonPhrase = ex.HttpResponseMessage.ReasonPhrase;

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                ReasonPhrase = reasonPhrase
            }.ToString());
        }
    }
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ReasonPhrase { get; set; }
        public string ErrorCode { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}