using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Commands.Likes
{
    public class RemoveLikeFromPostRequest : IRequest<Result<object>>
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }

    public class RemoveLikeFromPostRequestHandler : IRequestHandler<RemoveLikeFromPostRequest, Result<object>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveLikeFromPostRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> Handle(RemoveLikeFromPostRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<object>.Failure(nameof(request.UserId), "User doesn't exist.");
            }

            if (user.IsBanned)
            {
                return Result<object>.Failure(string.Empty, "You are banned.");
            }

            var post = await _unitOfWork.PostRepository.GetPostAsync(request.PostId);
            if (post == null)
            {
                return Result<object>.Failure(nameof(request.PostId), "Post doesn't exist");
            }

            var like = post.PostLikes.SingleOrDefault(pl => pl.UserId == user.Id);
            if (like == null)
            {
                return Result<object>.Failure(nameof(request.PostId), "This post isn't liked");
            }

            await _unitOfWork.LikeRepository.RemoveLikeAsync(post.Id, user.Id);

            return Result<object>.Success();
        }
    }
}