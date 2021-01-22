using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Common;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;
using YAFF.Data;

namespace YAFF.Business.Commands.Comments
{
    public class DeleteCommentRequest : IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }

    public class DeleteCommentRequestHandler : IRequestHandler<DeleteCommentRequest, Result<object>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly UserManager<User> _userManager;

        public DeleteCommentRequestHandler(ForumDbContext forumDbContext, UserManager<User> userManager)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
        }

        public async Task<Result<object>> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result<object>.Failure();
            }

            var comment = await _forumDbContext.Comments.FindAsync(request.CommentId);
            if (comment == null)
            {
                return Result<object>.Failure(nameof(request.CommentId), "Comment doesn't exist");
            }

            if (comment.AuthorId != user.Id)
            {
                return Result<object>.Failure();
            }

            _forumDbContext.Comments.Remove(comment);
            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}