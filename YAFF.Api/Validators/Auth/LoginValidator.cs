using FluentValidation;
using YAFF.Api.DTO.Auth;

namespace YAFF.Api.Validators.Auth
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