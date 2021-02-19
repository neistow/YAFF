using System;
using System.Collections.Generic;
using System.Linq;

namespace YAFF.Core.DTO
{
    public record PostDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public DateTime DateAdded { get; init; }
        public DateTime? DateEdited { get; init; }
        public int LikesCount;

        public PostPreviewDto Preview { get; init; }
        public AuthorDto Author { get; init; }

        public IEnumerable<string> Tags { get; init; } = new List<string>();
    }
}