using System;
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
            RuleFor(p => p.Title).MinimumLength(5).MaximumLength(255);
            RuleFor(p => p.Body).MinimumLength(250);
            RuleFor(p => p.Tags).Must(BeUnique).WithMessage("Tags must be unique");
        }

        private bool BeUnique(List<Guid> tags)
        {
            var distinctTags = tags.Distinct().ToList();
            return distinctTags.Count == tags.Count;
        }
    }
}