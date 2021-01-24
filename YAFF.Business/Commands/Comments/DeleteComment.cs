using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
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

        public DeleteCommentRequestHandler(ForumDbContext forumDbContext)
        {
            _forumDbContext = forumDbContext;
        }

        public async Task<Result<object>> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
        {
            var comment = await _forumDbContext.Comments
                .SingleOrDefaultAsync(c => c.Id == request.CommentId && c.AuthorId == request.UserId);
            if (comment == null)
            {
                return Result<object>.Failure(nameof(request.CommentId), "Comment doesn't exist or you can't edit it");
            }

            _forumDbContext.Comments.Remove(comment);
            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}