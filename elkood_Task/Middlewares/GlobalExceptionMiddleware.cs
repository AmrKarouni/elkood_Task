using ElKood.Application.Interfaces;
using System.Net;
using System.Text.Json;

namespace elkood_Task.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        public GlobalExceptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var appLogger = scope.ServiceProvider.GetRequiredService<IAppLogService>();
                    await appLogger.LogEvent(ElKood.Core.Enums.LogEventTypes.Exception, GetUserNameFromContext(httpContext), ex.Message);
                }

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code = exception switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new
            {
                error = exception.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private string GetUserNameFromContext(HttpContext context)
        {
            return context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name : "Anonymous";
        }
    }
}
