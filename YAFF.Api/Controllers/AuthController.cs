﻿using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YAFF.Api.DTO.Auth;
using YAFF.Api.Extensions;
using YAFF.Api.Helpers;
using YAFF.Business.Commands.Auth;
using YAFF.Business.Commands.Users;

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
            var username = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "Name");
            return Ok($"You are authorised as {username!.Value}");
        }

        [AllowAnonymous]
        [EnableTransaction]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await Mediator.Send(new CreateUserCommand
            {
                Email = registerDto.Email,
                NickName = registerDto.Nickname,
                Password = registerDto.Password
            });

            return !result.Succeeded
                ? BadRequest(result.ToApiError(400))
                : Ok(result.ToApiResponse(200));
        }

        [AllowAnonymous]
        [EnableTransaction]
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
                : Ok(result.ToApiResponse(200));
        }

        [EnableTransaction]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return Unauthorized();
            }

            var result =
                await Mediator.Send(new RefreshTokenCommand
                {
                    UserId = CurrentUserId,
                    RefreshToken = request.Token
                });

            return !result.Succeeded
                ? Unauthorized()
                : Ok(result.ToApiResponse(200));
        }
    }
}