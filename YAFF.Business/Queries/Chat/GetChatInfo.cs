using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Queries.Chat
{
    public class GetChatInfoRequest : IRequest<Result<ChatInfoDto>>
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
    }

    public class GetChatInfoRequestHandler : IRequestHandler<GetChatInfoRequest, Result<ChatInfoDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetChatInfoRequestHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ChatInfoDto>> Handle(GetChatInfoRequest request, CancellationToken cancellationToken)
        {
            var chat = await _forumDbContext.Chats
                .IncludeUsers()
                .SingleOrDefaultAsync(c => c.Id == request.ChatId);

            if (chat == null)
            {
                return Result<ChatInfoDto>.Failure(nameof(request.ChatId), "Chat doesn't exist");
            }

            if (chat.Users.SingleOrDefault(cu => cu.UserId == request.UserId) == null)
            {
                return Result<ChatInfoDto>.Failure(nameof(request.UserId), "You are not member of the chat");
            }

            var result = _mapper.Map<ChatInfoDto>(chat);
            return Result<ChatInfoDto>.Success(result);
        }
    }
}