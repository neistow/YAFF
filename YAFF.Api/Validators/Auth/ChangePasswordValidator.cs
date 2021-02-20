using FluentValidation;
using YAFF.Api.DTO.Auth;
using YAFF.Api.Extensions;

namespace YAFF.Api.Validators.Auth
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(cp => cp.OldPassword).NotEmpty();
            RuleFor(cp => cp.NewPassword).Password();
            RuleFor(cp => cp.NewPassword).NotEqual(cp => cp.OldPassword).WithMessage("Passwords cannot be equal");
        }
    }
}