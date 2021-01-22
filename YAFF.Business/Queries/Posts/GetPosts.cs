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

namespace YAFF.Business.Queries.Posts
{
    public class GetPostsQuery : IRequest<Result<PostListDto>>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, Result<PostListDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<PostListDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _forumDbContext.Posts
                .AsNoTracking()
                .IncludeLikes()
                .IncludeAuthor()
                .IncludeTags()
                .OrderByDescending(p => p.DateAdded)
                .Paginate(request.Page, request.PageSize)
                .ToListAsync();
            var allPostsCount = await _forumDbContext.Posts.CountAsync();
            
            var shortenedPosts = posts.Select(post =>
            {
                post.Body = string.Join(" ", post.Body.Split().Take(40));
                return post;
            });

            if (!shortenedPosts.Any())
            {
                return Result<PostListDto>.Failure(nameof(request.Page), "No records found");
            }

            var postDtos = _mapper.Map<IEnumerable<PostListItemDto>>(shortenedPosts);
            return Result<PostListDto>.Success(new PostListDto
            {
                Posts = postDtos,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int) Math.Ceiling(allPostsCount / (double) request.PageSize)
            });
        }
    }
}