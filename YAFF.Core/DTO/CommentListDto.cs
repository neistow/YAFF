using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public record CommentListDto
    {
        public IEnumerable<PostCommentDto> Comments { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}