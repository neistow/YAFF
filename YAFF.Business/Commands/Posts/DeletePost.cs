using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Extensions;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Commands.Posts
{
    public class DeletePostCommand : IRequest<Result<object>>
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Result<object>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetPostAsync(request.PostId);
            if (post == null)
            {
                return Result<object>.Failure(nameof(request.PostId), "No post with such id");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<object>.Failure();
            }

            if (post.AuthorId != user.Id && !user.CanDeletePosts())
            {
                return Result<object>.Failure(nameof(request.PostId), "You are not allowed to delete post.");
            }

            await _unitOfWork.PostRepository.DeletePostAsync(post.Id);

            return Result<object>.Success();
        }
    }
}