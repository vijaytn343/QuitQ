using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace QuitQ.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "An error occurred while processing the request");

            var response = new
            {
                StatusCode = exception switch
                {
                    BadHttpRequestException =>
                        (int)HttpStatusCode.BadRequest,

                    KeyNotFoundException =>
                        (int)HttpStatusCode.NotFound,

                    _ =>
                        (int)HttpStatusCode.InternalServerError
                },

                Message = exception.Message
            };

            httpContext.Response.StatusCode = response.StatusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken);

            return true;
        }
    }
}
