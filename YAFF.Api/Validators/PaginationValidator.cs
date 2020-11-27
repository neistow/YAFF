using FluentValidation;
using YAFF.Api.DTO;

namespace YAFF.Api.Validators
{
    public class PaginationValidator : AbstractValidator<PaginationDto>
    {
        public PaginationValidator()
        {
            RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}