using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities.Identity;

namespace YAFF.Business.Commands.Auth
{
    public class AuthenticateUserCommand : IRequest<Result<UserAuthenticatedDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result<UserAuthenticatedDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticateUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager,
            IMediator mediator, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Result<UserAuthenticatedDto>> Handle(AuthenticateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<UserAuthenticatedDto>.Failure(nameof(request.Email),
                    "Invalid email");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Result<UserAuthenticatedDto>.Failure(nameof(request.Password), "Incorrect password");
            }

            if (user.IsBanned)
            {
                return Result<UserAuthenticatedDto>.Failure(string.Empty,
                    $"You are banned. Ban expiration date: {user.BanLiftDate}");
            }

            var jwtToken = await _mediator.Send(new GenerateJwtTokenCommand {User = user});

            return Result<UserAuthenticatedDto>.Success(new UserAuthenticatedDto
            {
                User = _mapper.Map<UserDto>(user),
                JwtToken = jwtToken,
            });
        }
    }
}