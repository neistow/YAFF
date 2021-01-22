using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;
using YAFF.Data;
using YAFF.Data.Extensions;

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
        private readonly UserManager<User> _userManager;

        public AddLikeToPostRequestHandler(ForumDbContext forumDbContext, UserManager<User> userManager)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
        }

        public async Task<Result<object>> Handle(AddLikeToPostRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result<object>.Failure(nameof(request.UserId), "User doesn't exist.");
            }

            if (user.IsBanned)
            {
                return Result<object>.Failure(string.Empty, "You are banned.");
            }

            var post = await _forumDbContext.Posts.FindAsync(user.Id);
            if (post == null)
            {
                return Result<object>.Failure(nameof(request.PostId), "Post doesn't exist");
            }

            var like = await _forumDbContext.PostLikes.FindAsync(user.Id, post.Id);
            if (like != null)
            {
                return Result<object>.Failure(nameof(request.PostId), "This post was liked");
            }

            var postLike = new PostLike {PostId = post.Id, UserId = user.Id};
            await _forumDbContext.PostLikes.AddAsync(postLike);

            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}