using FluentValidation;
using YAFF.Api.DTO;

namespace YAFF.Api.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenValidator()
        {
            RuleFor(r => r.Token).NotEmpty();
        }
    }
}