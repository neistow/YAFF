using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public record CommentListDto
    {
        public IEnumerable<CommentDto> Comments { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
    }
}