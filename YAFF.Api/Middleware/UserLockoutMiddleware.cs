using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Entities.Identity;

namespace YAFF.Api.Middleware
{
    public class UserLockoutMiddleware
    {
        private readonly RequestDelegate _next;

        public UserLockoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserManager<User> userManager)
        {
            var userId = httpContext.User.Claims.SingleOrDefault(c => c.Type == "Id");
            if (userId != null)
            {
                var user = await userManager.FindByIdAsync(userId.Value);
                if (user == null || user.IsBanned)
                {
                    await HandleErrorAsync(httpContext);
                    return;
                }
            }

            await _next(httpContext);
        }

        private Task HandleErrorAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.Forbidden;

            return context.Response.WriteAsync("Your account is banned or deleted.");
        }
    }
}