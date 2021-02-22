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

namespace YAFF.Business.Queries.Posts
{
    public class GetUserPostsQuery : IRequest<Result<PagedList<PostListItemDto>>>
    {
        public int UserId { get; set; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, Result<PagedList<PostListItemDto>>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetUserPostsQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<PostListItemDto>>> Handle(GetUserPostsQuery request,
            CancellationToken cancellationToken)
        {
            var posts = await _forumDbContext.Posts
                .IncludeLikes()
                .IncludeAuthor()
                .IncludeTags()
                .IncludePreview()
                .Where(p => p.AuthorId == request.UserId)
                .OrderByDescending(p => p.DateAdded)
                .Paginate(request.Page, request.PageSize)
                .AsNoTracking()
                .ToListAsync();
            var allPostsCount = await _forumDbContext.Posts.Where(p => p.AuthorId == request.UserId).CountAsync();

            if (!posts.Any())
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