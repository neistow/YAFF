using FluentValidation;
using YAFF.Api.DTO.Auth;
using YAFF.Api.Extensions;

namespace YAFF.Api.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(r => r.UserName).MinimumLength(2).MaximumLength(32);
            RuleFor(r => r.Email).EmailAddress();
            RuleFor(r => r.Password).Password();
        }
    }
}