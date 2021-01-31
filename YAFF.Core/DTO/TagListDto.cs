using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public class TagListDto
    {
        public IEnumerable<TagDto> Tags { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
    }
}