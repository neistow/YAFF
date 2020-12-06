using Microsoft.AspNetCore.Builder;
using YAFF.Api.Middleware;

namespace YAFF.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
        }
    }
}