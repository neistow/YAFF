using Microsoft.AspNetCore.Builder;
using YAFF.Api.Middleware;

namespace YAFF.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
        }

        public static void UseLockoutMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserLockoutMiddleware>();
        }
    }
}