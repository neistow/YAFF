using FluentValidation;
using YAFF.Api.DTO.Chat;

namespace YAFF.Api.Validators.Chat
{
    public class SentMessageValidator : AbstractValidator<SentMessageDto>
    {
        public SentMessageValidator()
        {
            RuleFor(s => s.ChatId).GreaterThan(0);
            RuleFor(s => s.Message).NotEmpty().MaximumLength(4096);
        }
    }
}