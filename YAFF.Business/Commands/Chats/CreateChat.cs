using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Data;

namespace YAFF.Business.Commands.Chats
{
    public class CreateChatCommand : IRequest<Result<ChatInfoDto>>
    {
        public int CreatorId { get; set; }
        public bool IsPrivate { get; set; }
        public IEnumerable<int> Users { get; set; }
    }

    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Result<ChatInfoDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public CreateChatCommandHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ChatInfoDto>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var chat = new Chat
            {
                IsPrivate = request.IsPrivate,
                Users = request.Users.Where(i => i != request.CreatorId).Select(i => new ChatUser {UserId = i}).ToList()
            };
            chat.Users.Add(new ChatUser {UserId = request.CreatorId});

            if (chat.Users.Count == 1)
            {
                return Result<ChatInfoDto>.Failure(nameof(request.Users),"Chat should have at least two users");
            }
            
            await _forumDbContext.Chats.AddAsync(chat);
            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<ChatInfoDto>(chat);
            return Result<ChatInfoDto>.Success(result);
        }
    }
}