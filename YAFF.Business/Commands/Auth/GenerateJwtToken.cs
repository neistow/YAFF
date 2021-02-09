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
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;

namespace YAFF.Business.Commands.Auth
{
    public class GenerateJwtTokenCommand : IRequest<string>
    {
        public User User { get; init; }
        public IList<string> Roles { get; init; }
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
                new Claim("Id", request.User.Id.ToString()),
                new Claim("UserName", request.User.UserName),
                new Claim("Email", request.User.Email)
            };
            claims.Add(new Claim("Roles", string.Join(",", request.Roles)));

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret)),
                SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_tokenConfig.AccessTokenExpirationMinutes),
                SigningCredentials = credentials,
                Issuer = _tokenConfig.Issuer,
                Audience = _tokenConfig.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}