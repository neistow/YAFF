using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.Interfaces;
using YAFF.Data;
using YAFF.Data.Extensions;

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
        private readonly IPhotoStorage _photoStorage;

        public DeletePostCommandHandler(ForumDbContext forumDbContext, IPhotoStorage photoStorage)
        {
            _forumDbContext = forumDbContext;
            _photoStorage = photoStorage;
        }

        public async Task<Result<object>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _forumDbContext.Posts
                .IncludePreview()
                .SingleOrDefaultAsync(p => p.AuthorId == request.UserId && p.Id == request.PostId);
            if (post == null)
            {
                return Result<object>.Failure(nameof(request.PostId), "Post doesn't exist or you can't edit it");
            }

            _forumDbContext.Posts.Remove(post);
            await _photoStorage.DeletePhotoAsync(post.Preview.Image.FileName);

            await _forumDbContext.SaveChangesAsync();

            return Result<object>.Success();
        }
    }
}