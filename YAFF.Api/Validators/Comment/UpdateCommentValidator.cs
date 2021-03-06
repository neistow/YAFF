﻿using FluentValidation;
using YAFF.Api.DTO.Comment;

namespace YAFF.Api.Validators.Comment
{
    public class UpdateCommentValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentValidator()
        {
            RuleFor(pc => pc.Body).NotEmpty().MaximumLength(1024);
        }
    }
}