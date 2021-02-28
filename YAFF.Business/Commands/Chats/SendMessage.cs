using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Commands.Chats
{
    public class SendMessageCommand : IRequest<Result<ChatMessageDto>>
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Message { get; set; }
    }

    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result<ChatMessageDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public SendMessageCommandHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ChatMessageDto>> Handle(SendMessageCommand request,
            CancellationToken cancellationToken)
        {
            var chat = await _forumDbContext.Chats
                .IncludeUsers()
                .SingleOrDefaultAsync(c => c.Id == request.ChatId);
            if (chat == null)
            {
                return Result<ChatMessageDto>.Failure(nameof(request.ChatId), "Chat doesn't exist");
            }

            var user = chat.Users.SingleOrDefault(cu => cu.UserId == request.SenderId);
            if (user == null)
            {
                return Result<ChatMessageDto>.Failure(nameof(request.ChatId), "You are not a participant of this chat");
            }

            var message = new ChatMessage
            {
                SenderId = request.SenderId,
                Text = request.Message,
                DateSent = DateTime.UtcNow
            };
            chat.Messages.Add(message);

            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<ChatMessageDto>(message);
            return Result<ChatMessageDto>.Success(result);
        }
    }
}