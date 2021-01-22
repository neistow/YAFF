using System;

namespace YAFF.Core.DTO
{
    public record CommentDto
    {
        public int Id { get; init; }
        public string Body { get; init; }
        public DateTime DateAdded { get; init; }
        public DateTime? DateEdited { get; init; }

        public AuthorDto Author { get; init; }

        public int PostId { get; init; }
        public int? ReplyTo { get; init; }
    }
}