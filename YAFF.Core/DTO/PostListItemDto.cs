using System;
using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public record PostListItemDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public DateTime DateAdded { get; init; }
        public DateTime? DateEdited { get; init; }
        public int LikesCount { get; init; }

        public IEnumerable<string> Tags { get; init; }
        public string PreviewImage { get; init; }
        public AuthorDto Author { get; init; }
    }
}