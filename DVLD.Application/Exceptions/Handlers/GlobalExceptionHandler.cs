using DVLD.Application.Common.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
namespace DVLD.Application.Exceptions.Handlers
{
    public class GlobalExceptionHandler : ResponseHandler, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(Fail(exception.Data, exception.Message));
            return true;
        }
    }
}
