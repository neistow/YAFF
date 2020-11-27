using FluentValidation;
using YAFF.Api.DTO;

namespace YAFF.Api.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(r => r.Nickname).MinimumLength(5).MaximumLength(55);
            RuleFor(r => r.Email).EmailAddress();
            RuleFor(r => r.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
        }
    }
}