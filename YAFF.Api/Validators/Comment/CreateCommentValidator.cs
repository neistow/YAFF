using FluentValidation;
using YAFF.Api.DTO.Comment;

namespace YAFF.Api.Validators.Comment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(pc => pc.PostId).NotEmpty();
            RuleFor(pc => pc.Body).NotEmpty().MaximumLength(1000);
        }
    }
}