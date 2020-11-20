using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO;
using YAFF.Api.Extensions;
using YAFF.Business.Commands.Users;

namespace YAFF.Api.Controllers
{
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

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
    }
}