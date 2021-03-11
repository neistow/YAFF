using System.Collections.Generic;
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
    public class CreateGroupChatCommand : IRequest<Result<ChatInfoDto>>
    {
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public List<int> ChatUsers { get; set; }
    }

    public class CreateGroupChatCommandHandler : IRequestHandler<CreateGroupChatCommand, Result<ChatInfoDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public CreateGroupChatCommandHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ChatInfoDto>> Handle(CreateGroupChatCommand request, CancellationToken cancellationToken)
        {
            var userIds = request.ChatUsers.ToHashSet();
            userIds.Add(request.CreatorId);

            var users = await _forumDbContext.Users
                .IncludeProfile()
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            if (users.Count != userIds.Count)
            {
                return Result<ChatInfoDto>.Failure(nameof(request.ChatUsers), "Chat contains non existent user(s)");
            }

            var chat = new GroupChat
            {
                Title = request.Title,
                Users = users.Select(u => new ChatUser {User = u}).ToList()
            };
            await _forumDbContext.AddAsync(chat);
            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<ChatInfoDto>(chat);
            return Result<ChatInfoDto>.Success(result);
        }
    }
}