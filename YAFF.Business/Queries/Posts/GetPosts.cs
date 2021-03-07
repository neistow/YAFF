using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Business.Specifications;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Extensions;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Queries.Posts
{
    public class GetPostsQuery : IRequest<Result<PagedList<PostListItemDto>>>
    {
        public IEnumerable<int> IncludeTags { get; init; }
        public FilterMode InclusionMode { get; init; }
        public IEnumerable<int> ExcludeTags { get; init; }
        public FilterMode ExclusionMode { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, Result<PagedList<PostListItemDto>>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<PostListItemDto>>> Handle(GetPostsQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new PostHasTagsSpecification(request.IncludeTags, request.InclusionMode,
                request.ExcludeTags, request.ExclusionMode);

            var posts = await _forumDbContext.Posts
                .IncludeLikes()
                .IncludeAuthor()
                .IncludeTags()
                .IncludePreview()
                .Where(spec.Expression)
                .OrderByDescending(p => p.DateAdded)
                .Paginate(request.Page, request.PageSize)
                .AsNoTracking()
                .ToListAsync();
            var allPostsCount = await _forumDbContext.Posts.Where(spec.Expression).CountAsync();

            if (!posts.Any() && request.Page > 1)
            {
                return Result<PagedList<PostListItemDto>>.Failure(nameof(request.Page), "No records found");
            }

            var postDtos = _mapper.Map<IEnumerable<PostListItemDto>>(posts);

            return Result<PagedList<PostListItemDto>>.Success(
                postDtos.ToPagedList(
                    request.Page,
                    request.PageSize,
                    (int) Math.Ceiling(allPostsCount / (double) request.PageSize)));
        }
    }
}