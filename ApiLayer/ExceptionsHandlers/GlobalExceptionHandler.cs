using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using static DVLDApi.Helpers.ApiResponse;
namespace ApiLayer.ExceptionsHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(CreateResponse(StatusFail, exception.Message, exception.Data)));
            return true;
        }
    }
}
