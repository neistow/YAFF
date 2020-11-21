using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YAFF.Core.Configs;

namespace YAFF.Business.Commands.Auth
{
    public class GenerateJwtTokenCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class GenerateJwtTokenCommandHandler : IRequestHandler<GenerateJwtTokenCommand, string>
    {
        private readonly JwtTokenConfig _tokenConfig;

        public GenerateJwtTokenCommandHandler(IOptions<JwtTokenConfig> options)
        {
            _tokenConfig = options.Value;
        }

        public Task<string> Handle(GenerateJwtTokenCommand request, CancellationToken cancellationToken)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.UserId.ToString()),
                new Claim(ClaimTypes.Email, request.UserEmail),
            };
            claims.AddRange(request.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var jwtToken = new JwtSecurityToken(
                _tokenConfig.Issuer,
                _tokenConfig.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenConfig.AccessTokenExpirationMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret)),
                    SecurityAlgorithms.HmacSha256Signature));

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }
    }
}