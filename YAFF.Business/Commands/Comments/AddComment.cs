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
    public class AddCommentRequest : IRequest<Result<PostCommentDto>>
    {
        public Guid PostId { get; set; }
        public string Body { get; set; }
        public Guid? ReplyTo { get; set; }
        public Guid AuthorId { get; set; }
    }

    public class AddCommentRequestHandler : IRequestHandler<AddCommentRequest, Result<PostCommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCommentRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PostCommentDto>> Handle(AddCommentRequest request, CancellationToken cancellationToken)
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

            var post = await _unitOfWork.PostRepository.GetPostAsync(request.PostId);
            if (post == null)
            {
                return Result<PostCommentDto>.Failure(nameof(request.PostId), "Post doesn't exist");
            }

            if (request.ReplyTo.HasValue)
            {
                var replyComment = await _unitOfWork.CommentRepository.GetCommentAsync(request.ReplyTo.Value);
                if (replyComment == null)
                {
                    return Result<PostCommentDto>.Failure(nameof(request.ReplyTo), "Comment doesn't exist");
                }
            }

            var comment = new PostComment
            {
                Body = request.Body,
                PostId = post.Id,
                Author = user,
                AuthorId = user.Id,
                ReplyTo = request.ReplyTo,
                DateCommented = DateTime.UtcNow
            };

            await _unitOfWork.CommentRepository.AddCommentAsync(comment);

            var result = _mapper.Map<PostCommentDto>(comment);
            return Result<PostCommentDto>.Success(result);
        }
    }
}