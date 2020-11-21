using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using YAFF.Core.Configs;
using YAFF.Core.Entities;

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