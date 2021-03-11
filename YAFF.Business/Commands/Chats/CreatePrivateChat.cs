using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Business.Specifications;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Data;

namespace YAFF.Business.Commands.Chats
{
    public class CreatePrivateChatCommand : IRequest<Result<ChatInfoDto>>
    {
        public int CreatorId { get; set; }
        public int PartnerId { get; set; }
    }

    public class CreatePrivateChatCommandHandler : IRequestHandler<CreatePrivateChatCommand, Result<ChatInfoDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public CreatePrivateChatCommandHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ChatInfoDto>> Handle(CreatePrivateChatCommand request, CancellationToken cancellationToken)
        {
            var creator = await _forumDbContext.Users.FindAsync(request.CreatorId);
            var partner = await _forumDbContext.Users.FindAsync(request.PartnerId);
            if (partner == null)
            {
                return Result<ChatInfoDto>.Failure(nameof(request.PartnerId), "User with such id doesn't exist");
            }

            if (creator.Id == partner.Id)
            {
                return Result<ChatInfoDto>.Failure("You can't create a chat with yourself");
            }

            var spec = new ChatHasUsersSpecification(new[] {request.CreatorId, request.PartnerId});

            var chatInDb = await _forumDbContext.PrivateChats
                .SingleOrDefaultAsync(spec.Expression);
            if (chatInDb != null)
            {
                return Result<ChatInfoDto>.Failure(string.Empty, "Such chat already exists");
            }

            var chat = new PrivateChat
            {
                Users = {new ChatUser {User = partner}, new ChatUser {User = creator}}
            };
            await _forumDbContext.AddAsync(chat);
            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<ChatInfoDto>(chat);
            return Result<ChatInfoDto>.Success(result);
        }
    }
}