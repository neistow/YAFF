using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.Entities;
using YAFF.Data;

namespace YAFF.Business.Commands.Chats
{
    public class LeaveChatCommand : IRequest<Result<object>>
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
    }

    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand, Result<object>>
    {
        private readonly ForumDbContext _forumDbContext;

        public LeaveChatCommandHandler(ForumDbContext forumDbContext)
        {
            _forumDbContext = forumDbContext;
        }

        public async Task<Result<object>> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            var chatUser = await _forumDbContext.ChatUsers.FindAsync(request.ChatId, request.UserId);
            if (chatUser == null)
            {
                return Result<object>.Failure(nameof(request.ChatId), "You are not member of the chat");
            }

            var chat = await _forumDbContext.Chats.FindAsync(request.ChatId);
            if (chat is PrivateChat)
            {
                return Result<object>.Failure(nameof(request.ChatId), "Can't leave a private chat");
            }

            _forumDbContext.ChatUsers.Remove(chatUser);
            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}