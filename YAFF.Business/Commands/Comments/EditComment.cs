using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Commands.Comments
{
    public class EditCommentRequest : IRequest<Result<PostCommentDto>>
    {
        public Guid CommentId { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
    }

    public class EditCommentRequestHandler : IRequestHandler<EditCommentRequest, Result<PostCommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditCommentRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PostCommentDto>> Handle(EditCommentRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.AuthorId);
            if (user == null)
            {
                return Result<PostCommentDto>.Failure();
            }

            if (user.IsBanned)
            {
                return Result<PostCommentDto>.Failure(string.Empty, "You are banned");
            }

            var comment = await _unitOfWork.CommentRepository.GetCommentAsync(request.CommentId);
            if (comment == null)
            {
                return Result<PostCommentDto>.Failure(nameof(request.CommentId), "Comment doesn't exist");
            }

            if (comment.AuthorId != user.Id)
            {
                return Result<PostCommentDto>.Failure();
            }

            comment.Author = user;
            comment.Body = request.Body;
            comment.DateEdited = DateTime.UtcNow;

            await _unitOfWork.CommentRepository.UpdateCommentAsync(comment);

            var result = _mapper.Map<PostCommentDto>(comment);
            return Result<PostCommentDto>.Success(result);
        }
    }
}