using FluentValidation;
using YAFF.Api.DTO.Post;

namespace YAFF.Api.Validators.Post
{
    public class PostQueryValidator : AbstractValidator<PostQueryDto>
    {
        public PostQueryValidator()
        {
            RuleFor(pq => pq.InclusionMode).IsInEnum();
            RuleFor(pq => pq.ExclusionMode).IsInEnum();
        }
    }
}