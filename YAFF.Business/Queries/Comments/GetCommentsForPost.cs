using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Queries.Comments
{
    public class GetCommentsOfPostRequest : IRequest<Result<CommentListDto>>
    {
        public Guid PostId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class
        GetCommentsOfPostRequestHandler : IRequestHandler<GetCommentsOfPostRequest, Result<CommentListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCommentsOfPostRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CommentListDto>> Handle(GetCommentsOfPostRequest request,
            CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetPostAsync(request.PostId);
            if (post == null)
            {
                return Result<CommentListDto>.Failure(nameof(request.PostId), "Post doesn't exist.");
            }

            var comments =
                await _unitOfWork.CommentRepository.GetCommentsOfPostAsync(post.Id, request.Page, request.PageSize);
            var commentsCount = await _unitOfWork.CommentRepository.GetCommentsCountForPost(post.Id);

            if (!comments.Any())
            {
                return Result<CommentListDto>.Failure(nameof(request.Page), "No comments found");
            }

            var result = _mapper.Map<IEnumerable<PostCommentDto>>(comments);
            return Result<CommentListDto>.Success(new CommentListDto
            {
                Comments = result,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int) Math.Ceiling(commentsCount / (double) request.PageSize)
            });
        }
    }
}