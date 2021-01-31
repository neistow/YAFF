using System.Collections.Generic;
using YAFF.Core.Common;

namespace YAFF.Api.DTO.Post
{
    public record PostQueryDto : PaginationDto
    {
        public IEnumerable<int> IncludeTags { get; init; } = new List<int>();
        public FilterMode InclusionMode { get; init; } = FilterMode.Or;
        public IEnumerable<int> ExcludeTags { get; init; } = new List<int>();
        public FilterMode ExclusionMode { get; init; } = FilterMode.Or;
    }
}