using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO.Auth;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Auth;

namespace YAFF.Api.Controllers
{
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            var username = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "UserName");
            return Ok($"You are authorised as {username!.Value}");
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
                ? BadRequest(result.ToApiError(400))
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
                ? BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse());
        }
    }
}