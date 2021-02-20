using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO.Auth;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Auth;
using YAFF.Business.Commands.Profiles;

namespace YAFF.Api.Controllers
{
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            await Mediator.Send(new UpdateLastLoginDateCommand
            {
                UserId = CurrentUserId!.Value
            });
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await Mediator.Send(new RegisterUserCommand
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                Password = registerDto.Password
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await Mediator.Send(new AuthenticateUserCommand
            {
                Email = loginDto.Email,
                Password = loginDto.Password
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError())
                : Ok(result.ToApiResponse());
        }

        [AllowAnonymous]
        [HttpPost("[action]/{userId:min(1)}/{token}")]
        public async Task<IActionResult> ConfirmEmail([FromRoute] int userId, [FromRoute] string token)
        {
            var result = await Mediator.Send(new ConfirmEmailCommand
            {
                UserId = userId,
                Token = token
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }
    }
}