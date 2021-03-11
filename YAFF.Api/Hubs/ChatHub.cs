using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using YAFF.Api.DTO.Chat;
using YAFF.Api.Hubs.Common;
using YAFF.Api.Hubs.Interfaces;
using YAFF.Api.Validators.Chat;
using YAFF.Business.Commands.Chats;
using YAFF.Business.Queries.Chat;

namespace YAFF.Api.Hubs
{
    public class ChatHub : HubBase<IChatHub>
    {
        public ChatHub(IMediator mediator) : base(mediator)
        {
        }

        public async Task SendMessage(SentMessageDto message)
        {
            var validator = new SentMessageValidator();
            var validationResult = validator.Validate(message);
            if (!validationResult.IsValid)
            {
                throw new HubException(JsonSerializer.Serialize(validationResult.Errors));
            }

            var result = await Mediator.Send(new SendMessageCommand
            {
                ChatId = message.ChatId,
                Message = message.Message.Trim(),
                SenderId = int.Parse(Context.UserIdentifier!)
            });

            ThrowIfNotSucceeded(result);

            var usersResult = await Mediator.Send(new GetChatUsersQuery
            {
                ChatId = message.ChatId
            });

            ThrowIfNotSucceeded(usersResult);

            var usersIds = usersResult.Data.Select(u => u.UserId.ToString()).ToList();
            await Clients.Users(usersIds).ReceiveMessage(result.Data);
        }
    }
}