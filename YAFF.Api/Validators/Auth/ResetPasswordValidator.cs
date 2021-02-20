using FluentValidation;
using YAFF.Api.DTO.Auth;

namespace YAFF.Api.Validators.Auth
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(rp => rp.Email).EmailAddress();
        }
    }
}