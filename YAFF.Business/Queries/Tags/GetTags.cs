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

namespace YAFF.Business.Queries.Tags
{
    public class GetTagsRequest : IRequest<Result<TagListDto>>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public class GetTagsQueryHandler : IRequestHandler<GetTagsRequest, Result<TagListDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<Result<TagListDto>> Handle(GetTagsRequest request, CancellationToken cancellationToken)
        {
            var tags = await _forumDbContext.Tags
                .OrderByDescending(t => _forumDbContext.PostTags.Where(pt => pt.TagId == t.Id).Count())
                .Paginate(request.Page, request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var allTagsCount = await _forumDbContext.Tags.CountAsync();

            if (!tags.Any() && request.Page > 1)
            {
                return Result<TagListDto>.Failure(nameof(request.Page), "No records found");
            }

            return Result<TagListDto>.Success(new TagListDto
            {
                Tags = _mapper.Map<IEnumerable<TagDto>>(tags),
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int) Math.Ceiling(allTagsCount / (double) request.PageSize)
            });
        }
    }
}