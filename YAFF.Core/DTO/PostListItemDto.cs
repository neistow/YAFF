using System;
using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public record PostListItemDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public DateTime DatePosted { get; init; }
        public DateTime? DateEdited { get; init; }
        public int LikesCount { get; init; }
        public Guid AuthorId { get; init; }
        public IEnumerable<string> Tags { get; init; } = new List<string>();
    }
}