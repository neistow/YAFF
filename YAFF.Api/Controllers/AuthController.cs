using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO;
using YAFF.Api.Extensions;
using YAFF.Api.Helpers;
using YAFF.Business.Commands.Auth;
using YAFF.Business.Commands.Users;

namespace YAFF.Api.Controllers
{
    [Authorize]
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(LoginDto loginDto)
        {
            var result = await Mediator.Send(new CreateUserCommand
            {
                Email = loginDto.Email,
                NickName = loginDto.Nickname,
                Password = loginDto.Password
            });

            return !result.Succeeded
                ? (IActionResult) BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await Mediator.Send(new AuthenticateUserCommand
            {
                Email = loginDto.Email,
                Password = loginDto.Password
            });

            return !result.Succeeded
                ? (IActionResult) BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }

        [EnableTransaction]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return Unauthorized();
            }

            // In jwt payload user name is set to user Id
            var userId = HttpContext.User.Identity.Name;
            var result =
                await Mediator.Send(new RefreshTokenCommand
                    {UserId = Guid.Parse(userId), RefreshToken = request.Token});

            return !result.Succeeded
                ? (IActionResult) Unauthorized()
                : Ok(result.ToApiResponse(200));
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Yep");
        }
    }
}