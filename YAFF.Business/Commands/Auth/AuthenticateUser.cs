﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using YAFF.Business.Common;
using YAFF.Core.Common;
using YAFF.Core.Configs;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Business.Commands.Auth
{
    public class AuthenticateUserCommand : IRequest<Result<UserAuthenticatedDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result<UserAuthenticatedDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly JwtTokenConfig _jwtTokenConfig;

        public AuthenticateUserCommandHandler(IUnitOfWork unitOfWork, IMediator mediator,
            IOptions<JwtTokenConfig> options)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _jwtTokenConfig = options.Value;
        }

        public async Task<Result<UserAuthenticatedDto>> Handle(AuthenticateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<UserAuthenticatedDto>.Failure(nameof(request.Email),
                    "Invalid email");
            }

            if (user.IsBanned)
            {
                return Result<UserAuthenticatedDto>.Failure(string.Empty, "You are banned.");
            }

            // TODO : Uncomment when email confirmation is ready
            // if (!user.EmailConfirmed)
            // {
            //     return Result<UserAuthenticatedDto>.Failure(nameof(request.Email),
            //         "Please confirm email before logging in");
            // }

            if (!PasswordHasher.VerifyPasswordHash(request.Password, user.PasswordHash))
            {
                return Result<UserAuthenticatedDto>.Failure(request.Password, "Invalid password");
            }

            var userRoles = await _unitOfWork.RoleRepository.GetUserRoles(user.Id);
            var jwtToken = await _mediator.Send(new GenerateJwtTokenCommand
            {
                UserId = user.Id,
                UserName = user.NickName,
                UserEmail = user.Email,
                Roles = userRoles.Select(r => r.Name)
            });

            var tokenString = await _mediator.Send(new GenerateRefreshTokenCommand());
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                DateExpires = DateTime.UtcNow.AddMinutes(_jwtTokenConfig.RefreshTokenExpirationMinutes),
                Token = tokenString,
                UserId = user.Id
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);

            return Result<UserAuthenticatedDto>.Success(new UserAuthenticatedDto
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            });
        }
    }
}