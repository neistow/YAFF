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
using YAFF.Core.Extensions;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Queries.Comments
{
    public class GetCommentsOfPostQuery : IRequest<Result<PagedList<CommentDto>>>
    {
        public int PostId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetCommentsOfPostQueryHandler : IRequestHandler<GetCommentsOfPostQuery, Result<PagedList<CommentDto>>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetCommentsOfPostQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<CommentDto>>> Handle(GetCommentsOfPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _forumDbContext.Posts.FindAsync(request.PostId);
            if (post == null)
            {
                return Result<PagedList<CommentDto>>.Failure(nameof(request.PostId), "Post doesn't exist.");
            }

            var comments = await _forumDbContext.Comments
                .IncludeAuthor()
                .Where(c => c.PostId == request.PostId)
                .OrderByDescending(c => c.DateAdded)
                .Paginate(request.Page, request.PageSize)
                .AsNoTracking()
                .ToListAsync();
            var allCommentsCount = await _forumDbContext.Comments
                .Where(c => c.PostId == request.PostId)
                .CountAsync();

            if (!comments.Any() && request.Page > 1)
            {
                return Result<PagedList<CommentDto>>.Failure(nameof(request.Page), "No comments found");
            }

            var result = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Result<PagedList<CommentDto>>.Success(
                result.ToPagedList(
                    request.Page,
                    request.PageSize,
                    (int) Math.Ceiling(allCommentsCount / (double) request.PageSize)));
        }
    }
}