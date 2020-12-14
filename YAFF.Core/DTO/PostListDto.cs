using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public record PostListDto
    {
        public IEnumerable<PostListItemDto> Posts { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}