using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Commands.Comments
{
    public class DeleteCommentRequest : IRequest<Result<object>>
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }

    public class DeleteCommentRequestHandler : IRequestHandler<DeleteCommentRequest, Result<object>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<object>.Failure();
            }

            var comment = await _unitOfWork.CommentRepository.GetCommentAsync(request.CommentId);
            if (comment == null)
            {
                return Result<object>.Failure(nameof(request.CommentId), "Comment doesn't exist");
            }

            if (comment.AuthorId != user.Id)
            {
                return Result<object>.Failure();
            }

            await _unitOfWork.CommentRepository.DeleteCommentAsync(comment.Id);
            return Result<object>.Success();
        }
    }
}