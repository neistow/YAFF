using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Extensions;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Queries.Chat
{
    public class GetChatMessagesQuery : IRequest<Result<PagedList<ChatMessageDto>>>
    {
        public int ChatId { get; init; }
        public int UserId { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, Result<PagedList<ChatMessageDto>>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetChatMessagesQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ChatMessageDto>>> Handle(GetChatMessagesQuery request,
            CancellationToken cancellationToken)
        {
            var chatUser = await _forumDbContext.ChatUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(cu => cu.UserId == request.UserId && cu.ChatId == request.ChatId);
            if (chatUser == null)
            {
                return Result<PagedList<ChatMessageDto>>.Failure(nameof(request.ChatId),
                    "You are not member of this chat");
            }

            var messages = await _forumDbContext.ChatMessages
                .Where(cm => cm.ChatId == request.ChatId)
                .OrderBy(cm => cm.DateSent)
                .Paginate(request.Page, request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var allMessagesCount = await _forumDbContext.ChatMessages
                .Where(cm => cm.ChatId == request.ChatId)
                .CountAsync();

            if (!messages.Any())
            {
                return Result<PagedList<ChatMessageDto>>.Failure(nameof(request.Page), "No messages found");
            }

            var result = _mapper.Map<List<ChatMessageDto>>(messages);
            return Result<PagedList<ChatMessageDto>>.Success(
                result.ToPagedList(request.Page,
                    request.PageSize,
                    (int) Math.Ceiling(allMessagesCount / (double) request.PageSize)));
        }
    }
}