using FluentValidation;
using YAFF.Api.DTO.Profile;

namespace YAFF.Api.Validators.Profile
{
    public class EditProfileValidator : AbstractValidator<UpdateProfileDto>
    {
        public EditProfileValidator()
        {
            RuleFor(r => r.Bio).NotEmpty().MaximumLength(256);
            RuleFor(r => r.UserStatus).NotEmpty().MaximumLength(128);
            RuleFor(r => r.ProfileType).IsInEnum();
        }
    }
}