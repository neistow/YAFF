using FluentValidation;
using YAFF.Api.DTO;

namespace YAFF.Api.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(r => r.Email).EmailAddress();
            RuleFor(r => r.Password).NotEmpty();
        }
    }
}