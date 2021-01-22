using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace YAFF.Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation(
                    "Request {method} {url} => {statusCode} handled in {timeElapsed}ms",
                    httpContext.Request.Method,
                    httpContext.Request.Path.Value,
                    httpContext.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}