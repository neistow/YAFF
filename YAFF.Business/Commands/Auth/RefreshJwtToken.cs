using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;
using Microsoft.Extensions.Options;
using YAFF.Core.Common;
using YAFF.Core.Configs;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Business.Commands.Auth
{
    public class RefreshTokenCommand : IRequest<Result<UserAuthenticatedDto>>
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<UserAuthenticatedDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly JwtTokenConfig _jwtTokenConfig;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IOptions<JwtTokenConfig> options)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _jwtTokenConfig = options.Value;
        }

        public async Task<Result<UserAuthenticatedDto>> Handle(RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user.Id == Guid.Empty)
            {
                return Result<UserAuthenticatedDto>.Failure();
            }

            var token = await _unitOfWork.RefreshTokenRepository.FindToken(request.UserId, request.RefreshToken);
            if (token.Id == Guid.Empty)
            {
                return Result<UserAuthenticatedDto>.Failure();
            }

            if (TokenExpired(token))
            {
                return Result<UserAuthenticatedDto>.Failure();
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
            await _unitOfWork.RefreshTokenRepository.DeleteAsync(token.Id);
            return Result<UserAuthenticatedDto>.Success(new UserAuthenticatedDto
                {JwtToken = jwtToken, RefreshToken = refreshToken.Token});
        }

        private bool TokenExpired(RefreshToken token) => DateTime.UtcNow >= token.DateExpires;
    }
}