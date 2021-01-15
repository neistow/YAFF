using System;
using System.Collections.Generic;

namespace YAFF.Core.Entities
{
    public record Post
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public DateTime DateAdded { get; init; }
        public DateTime? DateEdited { get; init; }
        public int LikesCount { get; init; }

        public Guid AuthorId { get; init; }
        public User Author { get; init; }

        public ICollection<Tag> Tags { get; init; } = new List<Tag>();
        public IEnumerable<PostLike> PostLikes { get; init; } = new List<PostLike>();
        public IEnumerable<PostComment> PostComments { get; init; } = new List<PostComment>();
    }
}