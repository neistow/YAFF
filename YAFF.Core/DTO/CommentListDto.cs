using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public class CommentListDto
    {
        public IEnumerable<PostCommentDto> Comments { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}