using FluentValidation;
using YAFF.Api.DTO.Auth;
using YAFF.Api.Extensions;

namespace YAFF.Api.Validators.Auth
{
    public class ConfirmResetPasswordValidator : AbstractValidator<ConfirmResetPasswordDto>
    {
        public ConfirmResetPasswordValidator()
        {
            RuleFor(cr => cr.Token).NotEmpty();
            RuleFor(cr => cr.NewPassword).Password();
            RuleFor(cr => cr.Email).EmailAddress();
        }
    }
}