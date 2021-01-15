using System;

namespace YAFF.Core.Entities
{
    public record PostComment
    {
        public Guid Id { get; init; }
        public string Body { get; init; }
        public DateTime DateAdded { get; init; }
        public DateTime? DateEdited { get; init; }
        
        public Guid PostId { get; init; }
        public Post Post { get; init; }

        public Guid AuthorId { get; init; }
        public User Author { get; init; }

        public Guid? ReplyTo { get; init; }
        public PostComment Comment { get; init; }
    }
}