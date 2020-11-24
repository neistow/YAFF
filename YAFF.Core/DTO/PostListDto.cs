using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public class PostListDto
    {
        public IEnumerable<PostListItemDto> Posts { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}