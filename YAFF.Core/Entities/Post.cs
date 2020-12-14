using System;
using System.Collections.Generic;

namespace YAFF.Core.Entities
{
    public record Post
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public DateTime DatePosted { get; init; }
        public DateTime? DateEdited { get; init; }
        public int LikesCount { get; init; }

        public Guid AuthorId { get; init; }
        public User User { get; init; }

        public List<Tag> Tags { get; init; } = new();
        public List<PostLike> PostLikes { get; init; } = new();
        public List<PostComment> PostComments { get; init; } = new();
    }
}