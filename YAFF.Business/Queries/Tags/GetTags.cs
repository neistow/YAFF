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

namespace YAFF.Business.Queries.Tags
{
    public class GetTagsRequest : IRequest<Result<PagedList<TagDto>>>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public class GetTagsQueryHandler : IRequestHandler<GetTagsRequest, Result<PagedList<TagDto>>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<TagDto>>> Handle(GetTagsRequest request, CancellationToken cancellationToken)
        {
            var tags = await _forumDbContext.Tags
                .OrderByDescending(t => _forumDbContext.PostTags.Count(pt => pt.TagId == t.Id))
                .Paginate(request.Page, request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var allTagsCount = await _forumDbContext.Tags.CountAsync();

            if (!tags.Any() && request.Page > 1)
            {
                return Result<PagedList<TagDto>>.Failure(nameof(request.Page), "No records found");
            }

            var result = _mapper.Map<IEnumerable<TagDto>>(tags);
            return Result<PagedList<TagDto>>.Success(result.ToPagedList(request.Page, request.PageSize, allTagsCount));
        }
    }
}