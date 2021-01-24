using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.Entities;
using YAFF.Data;

namespace YAFF.Business.Commands.Likes
{
    public class AddLikeToPostRequest : IRequest<Result<object>>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }

    public class AddLikeToPostRequestHandler : IRequestHandler<AddLikeToPostRequest, Result<object>>
    {
        private readonly ForumDbContext _forumDbContext;

        public AddLikeToPostRequestHandler(ForumDbContext forumDbContext)
        {
            _forumDbContext = forumDbContext;
        }

        public async Task<Result<object>> Handle(AddLikeToPostRequest request, CancellationToken cancellationToken)
        {
            var post = await _forumDbContext.Posts.FindAsync(request.PostId);
            if (post == null)
            {
                return Result<object>.Failure(nameof(request.PostId), "Post doesn't exist");
            }

            var like = await _forumDbContext.PostLikes
                .SingleOrDefaultAsync(pl => pl.PostId == request.PostId && pl.UserId == request.UserId);
            if (like != null)
            {
                return Result<object>.Failure(nameof(request.PostId), "This post was liked");
            }

            var postLike = new PostLike {PostId = post.Id, UserId = request.UserId};
            await _forumDbContext.PostLikes.AddAsync(postLike);

            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}