using System;
using System.Collections.Generic;
using System.Linq;

namespace YAFF.Core.DTO
{
    public record PostDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public DateTime DatePosted { get; init; }
        public DateTime? DateEdited { get; init; }
        public int LikesCount => PostLikes.Count();

        public AuthorDto Author { get; init; }

        public IEnumerable<string> Tags { get; init; } = new List<string>();
        public IEnumerable<Guid> PostLikes { get; init; } = new List<Guid>();
    }
}