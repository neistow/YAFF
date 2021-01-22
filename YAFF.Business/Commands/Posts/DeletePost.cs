using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.Entities.Identity;
using YAFF.Data;

namespace YAFF.Business.Commands.Posts
{
    public class DeletePostCommand : IRequest<Result<object>>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Result<object>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly UserManager<User> _userManager;

        public DeletePostCommandHandler(ForumDbContext forumDbContext, UserManager<User> userManager)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
        }

        public async Task<Result<object>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result<object>.Failure();
            }

            var post = await _forumDbContext.Posts
                .Where(p => p.AuthorId == user.Id && p.Id == request.PostId)
                .FirstOrDefaultAsync();
            if (post == null)
            {
                return Result<object>.Failure(nameof(request.PostId), "Post doesn't exist");
            }

            _forumDbContext.Posts.Remove(post);
            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}