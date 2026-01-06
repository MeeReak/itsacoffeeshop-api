using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using smakchet.application.Exceptions;

namespace smakchet.application.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await HandlerException.HandleExceptionAsync(context, ex);
            }
        }
    }
}
