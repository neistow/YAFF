using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO.Auth;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Auth;
using YAFF.Business.Commands.Profiles;
using YAFF.Core.DTO;

namespace YAFF.Api.Controllers
{
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(200)]
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
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
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
        [ProducesResponseType(typeof(UserAuthenticatedDto), 200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
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

        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await Mediator.Send(new ChangePasswordCommand
            {
                NewPassword = changePasswordDto.NewPassword,
                OldPassword = changePasswordDto.OldPassword,
                UserId = CurrentUserId!.Value
            });
            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await Mediator.Send(new RequestPasswordResetCommand
            {
                Email = resetPasswordDto.Email
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }

        [AllowAnonymous]
        [HttpPost("[action]/confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, IEnumerable<string>>), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ConfirmResetPasswordDto dto)
        {
            var result = await Mediator.Send(new ResetPasswordCommand
            {
                Email = dto.Email,
                Token = dto.Token,
                NewPassword = dto.NewPassword
            });

            return result.Succeeded
                ? Ok(result.ToApiResponse())
                : BadRequest(result.ToApiError());
        }
    }
}