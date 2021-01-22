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
    public class GetPostQuery : IRequest<Result<PostDto>>
    {
        public int Id { get; init; }
    }

    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, Result<PostDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetPostQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _forumDbContext.Posts
                .AsNoTracking()
                .IncludeAuthor()
                .IncludeLikes()
                .IncludeTags()
                .SingleOrDefaultAsync(p => p.Id == request.Id);
            if (post == null)
            {
                return Result<PostDto>.Failure(nameof(request.Id), "Post doesnt exist");
            }

            var result = _mapper.Map<PostDto>(post);
            return Result<PostDto>.Success(result);
        }
    }
}