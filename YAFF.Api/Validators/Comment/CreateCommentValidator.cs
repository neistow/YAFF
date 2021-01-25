using FluentValidation;
using YAFF.Api.DTO.Comment;

namespace YAFF.Api.Validators.Comment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(pc => pc.PostId).NotEmpty().GreaterThan(0);
            RuleFor(pc => pc.Body).NotEmpty().MaximumLength(1024);
        }
    }
}