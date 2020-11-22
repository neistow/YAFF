using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace YAFF.Business.Commands.Auth
{
    public class GenerateRefreshTokenCommand : IRequest<string>
    {
    }

    public class GenerateRefreshTokenCommandHandler : IRequestHandler<GenerateRefreshTokenCommand, string>
    {
        public Task<string> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }
    }
}