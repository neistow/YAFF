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
    public class GetCommentsOfPostQuery : IRequest<Result<CommentListDto>>
    {
        public int PostId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetCommentsOfPostQueryHandler : IRequestHandler<GetCommentsOfPostQuery, Result<CommentListDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetCommentsOfPostQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<CommentListDto>> Handle(GetCommentsOfPostQuery query,
            CancellationToken cancellationToken)
        {
            var post = await _forumDbContext.Posts.FindAsync(query.PostId);
            if (post == null)
            {
                return Result<CommentListDto>.Failure(nameof(query.PostId), "Post doesn't exist.");
            }

            var comments = await _forumDbContext.Comments
                .IncludeAuthor()
                .Where(c => c.PostId == query.PostId)
                .Paginate(query.Page, query.PageSize)
                .AsNoTracking()
                .ToListAsync();
            var allCommentsCount = await _forumDbContext.Comments
                .Where(c => c.PostId == query.PostId)
                .CountAsync();

            if (!comments.Any())
            {
                return Result<CommentListDto>.Failure(nameof(query.Page), "No comments found");
            }

            var result = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Result<CommentListDto>.Success(new CommentListDto
            {
                Comments = result,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalPages = (int) Math.Ceiling(allCommentsCount / (double) query.PageSize)
            });
        }
    }
}