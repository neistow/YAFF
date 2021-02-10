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
            CascadeMode = CascadeMode.Stop;
            
            RuleFor(p => p.Title).NotEmpty().MinimumLength(5).MaximumLength(256);
            RuleFor(p => p.Body).NotEmpty().MinimumLength(256);

            RuleFor(p => p.Tags).Must(ContainValidValues).WithMessage("Null values or empty strings are not valid tags");
            RuleFor(p => p.Tags).Must(BeUnique).WithMessage("No duplicate tags allowed");

            RuleFor(p => p.PreviewBody).NotEmpty().MinimumLength(100).MaximumLength(256);
            RuleFor(p => p.PreviewImage).NotNull().WithMessage("Post must have a preview image");
        }

        private bool BeUnique(List<string> tags)
        {
            var distinctTags = tags.Select(t => t.ToLowerInvariant()).Distinct().ToList();
            return distinctTags.Count == tags.Count;
        }

        private bool ContainValidValues(List<string> tags)
        {
            return tags.TrueForAll(t => !string.IsNullOrWhiteSpace(t));
        }
    }
}