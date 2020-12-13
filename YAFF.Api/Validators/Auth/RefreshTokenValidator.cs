using FluentValidation;
using YAFF.Api.DTO.Auth;

namespace YAFF.Api.Validators.Auth
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenValidator()
        {
            RuleFor(r => r.Token).NotEmpty();
        }
    }
}