using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using YAFF.Api.DTO.Post;

namespace YAFF.Api.Validators.Post
{
    public class CreatePostValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostValidator()
        {
            RuleFor(p => p.Title).NotEmpty().MinimumLength(5).MaximumLength(256);
            RuleFor(p => p.Body).NotEmpty().MinimumLength(256);
            When(p => p.Tags != null,
                () => { RuleFor(p => p.Tags).Must(BeUnique).WithMessage("Tags must be unique"); });
            RuleFor(p => p.PreviewBody).NotEmpty().MinimumLength(100).MaximumLength(256);
            RuleFor(p => p.PreviewImage).NotNull().WithMessage("Post must have a preview image");
        }

        private bool BeUnique(List<string> tags)
        {
            var distinctTags = tags.Select(t => t.ToLowerInvariant()).Distinct().ToList();
            return distinctTags.Count == tags.Count;
        }
    }
}