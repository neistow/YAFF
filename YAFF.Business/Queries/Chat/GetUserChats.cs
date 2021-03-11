using System.Collections.Generic;
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
    public class GetUserChatsRequest : IRequest<Result<List<ChatInfoDto>>>
    {
        public int UserId { get; set; }
    }

    public class GetUserChatsRequestHandler : IRequestHandler<GetUserChatsRequest, Result<List<ChatInfoDto>>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetUserChatsRequestHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<List<ChatInfoDto>>> Handle(GetUserChatsRequest request,
            CancellationToken cancellationToken)
        {
            var userChats = await _forumDbContext.ChatUsers
                .Where(cu => cu.UserId == request.UserId)
                .AsNoTracking()
                .Select(cu => cu.ChatId)
                .ToListAsync();

            var chats = await _forumDbContext.Chats
                .IncludeUsersWithProfiles()
                .Where(c => userChats.Contains(c.Id))
                .AsNoTracking()
                .ToListAsync();

            var result = _mapper.Map<List<ChatInfoDto>>(chats);
            return Result<List<ChatInfoDto>>.Success(result);
        }
    }
}