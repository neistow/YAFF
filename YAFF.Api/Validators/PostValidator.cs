using FluentValidation;
using YAFF.Api.DTO;

namespace YAFF.Api.Validators
{
    public class PostValidator : AbstractValidator<PostDto>
    {
        public PostValidator()
        {
            RuleFor(p => p.Title).MinimumLength(5).MaximumLength(255);
            RuleFor(p => p.Body).MinimumLength(250);
        }
    }
}