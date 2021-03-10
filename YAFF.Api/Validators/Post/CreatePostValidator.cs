using FluentValidation;
using YAFF.Api.DTO.Post;

namespace YAFF.Api.Validators.Post
{
    public class CreatePostValidator : PostValidator<CreatePostDto>
    {
        public CreatePostValidator()
        {
            RuleFor(p => p.PreviewImage).NotNull().WithMessage("Post must have a preview image");
        }
    }
}