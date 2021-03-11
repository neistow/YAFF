using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using YAFF.Api.DTO.Chat;

namespace YAFF.Api.Validators.Chat
{
    public class CreateChatValidator : AbstractValidator<CreateChatDto>
    {
        public CreateChatValidator()
        {
            RuleFor(c => c.ChatUsers).NotEmpty().WithMessage("Chat should have at least one member");
            RuleFor(c => c.ChatUsers).Must(BeUnique).WithMessage("Chat can't have duplicate members");
            When(c => c.IsPrivate,
                () =>
                {
                    RuleFor(c => c.Title).Empty().WithMessage("Only group chats can have a title");
                    RuleFor(c => c.ChatUsers.Count).Equal(1)
                        .WithMessage("Private chat can contain only one user (not including you)");
                });
            When(c => !c.IsPrivate, () => { RuleFor(c => c.Title).NotEmpty().WithMessage("Group chat must have a title"); });
        }

        private bool BeUnique(List<int> users)
        {
            return users.ToHashSet().Count == users.Count;
        }
    }
}