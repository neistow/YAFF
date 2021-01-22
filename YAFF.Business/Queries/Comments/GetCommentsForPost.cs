using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Queries.Comments
{
    public class GetCommentsOfPostRequest : IRequest<Result<CommentListDto>>
    {
        public int PostId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class
        GetCommentsOfPostRequestHandler : IRequestHandler<GetCommentsOfPostRequest, Result<CommentListDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetCommentsOfPostRequestHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<CommentListDto>> Handle(GetCommentsOfPostRequest request,
            CancellationToken cancellationToken)
        {
            var post = await _forumDbContext.Posts.FindAsync(request.PostId);
            if (post == null)
            {
                return Result<CommentListDto>.Failure(nameof(request.PostId), "Post doesn't exist.");
            }

            var comments = await _forumDbContext.Comments
                .AsNoTracking()
                .IncludeAuthor()
                .Paginate(request.Page, request.PageSize)
                .ToListAsync();
            var allCommentsCount = await _forumDbContext.Comments.CountAsync();

            if (!comments.Any())
            {
                return Result<CommentListDto>.Failure(nameof(request.Page), "No comments found");
            }

            var result = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Result<CommentListDto>.Success(new CommentListDto
            {
                Comments = result,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int) Math.Ceiling(allCommentsCount / (double) request.PageSize)
            });
        }
    }
}