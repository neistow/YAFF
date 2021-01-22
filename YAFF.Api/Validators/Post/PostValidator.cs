using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using YAFF.Api.DTO.Post;

namespace YAFF.Api.Validators.Post
{
    public class PostValidator : AbstractValidator<PostDto>
    {
        public PostValidator()
        {
            RuleFor(p => p.Title).NotEmpty().MinimumLength(5).MaximumLength(255);
            RuleFor(p => p.Body).NotEmpty().MinimumLength(250);
            When(p => p.Tags != null,
                () => { RuleFor(p => p.Tags).Must(BeUnique).WithMessage("Tags must be unique"); });
        }

        private bool BeUnique(List<string> tags)
        {
            var distinctTags = tags.Select(t => t.ToLowerInvariant()).Distinct().ToList();
            return distinctTags.Count == tags.Count;
        }
    }
}