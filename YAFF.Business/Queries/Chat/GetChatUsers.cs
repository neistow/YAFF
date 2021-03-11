using System.Collections.Generic;
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
    public class GetChatUsersQuery : IRequest<Result<List<ChatUserDto>>>
    {
        public int ChatId { get; set; }
    }

    public class GetChatUsersQueryHandler : IRequestHandler<GetChatUsersQuery, Result<List<ChatUserDto>>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetChatUsersQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<List<ChatUserDto>>> Handle(GetChatUsersQuery request,
            CancellationToken cancellationToken)
        {
            var chat = await _forumDbContext.Chats
                .IncludeUsersWithProfiles()
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == request.ChatId);
            if (chat == null)
            {
                return Result<List<ChatUserDto>>.Failure(nameof(request.ChatId), "Chat doesn't exist");
            }

            var result = _mapper.Map<List<ChatUserDto>>(chat.Users);
            return Result<List<ChatUserDto>>.Success(result);
        }
    }
}