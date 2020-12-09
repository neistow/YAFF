using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Queries.Comments
{
    public class GetCommentsOfPostRequest : IRequest<Result<IEnumerable<PostCommentDto>>>
    {
        public Guid PostId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class
        GetCommentsOfPostRequestHandler : IRequestHandler<GetCommentsOfPostRequest, Result<IEnumerable<PostCommentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCommentsOfPostRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PostCommentDto>>> Handle(GetCommentsOfPostRequest request,
            CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetPostAsync(request.PostId);
            if (post == null)
            {
                return Result<IEnumerable<PostCommentDto>>.Failure(nameof(request.PostId), "Post doesn't exist.");
            }

            var comments =
                await _unitOfWork.CommentRepository.GetCommentsOfPostAsync(post.Id, request.Page, request.PageSize);
            if (!comments.Any())
            {
                return Result<IEnumerable<PostCommentDto>>.Failure(nameof(request.Page), "No comments found");
            }

            var result = _mapper.Map<IEnumerable<PostCommentDto>>(comments);
            return Result<IEnumerable<PostCommentDto>>.Success(result);
        }
    }
}