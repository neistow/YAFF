using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Queries.Posts
{
    public class GetPostsQuery : IRequest<Result<PostListDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, Result<PostListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PostListDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetPostsAsync(request.Page, request.PageSize);
            
            var shortenedPosts = posts.Select(post =>
            {
                var bodySummary = post.Body.Split().Take(40);
                return post with {Body = $"{string.Join(' ', bodySummary)}..."};
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
                PageSize = request.PageSize
            });
        }
    }
}