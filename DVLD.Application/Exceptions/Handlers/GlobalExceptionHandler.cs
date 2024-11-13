using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace DVLD.Application.Exceptions.Handlers
{
    public class GlobalExceptionHandler : ResponseHandler, IExceptionHandler
    {
        private readonly ILogger _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(Fail(exception.Data, exception.Message));
            _logger.LogCritical(exception, "An unhandled exception occurred: {Message}", exception.Message);
            return true;
        }
    }
}
