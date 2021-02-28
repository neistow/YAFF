using System.Linq;
using FluentValidation;
using YAFF.Api.DTO.Chat;

namespace YAFF.Api.Validators.Chat
{
    public class CreateChatValidator : AbstractValidator<CreateChatDto>
    {
        public CreateChatValidator()
        {
            RuleFor(c => c.ChatUsers).NotEmpty();
            When(c => c.IsPrivate, () => { RuleFor(c => c.ChatUsers.Count()).Equal(1); });
        }
    }
}