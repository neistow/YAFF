using System;

namespace YAFF.Core.DTO
{
    public record PostCommentDto
    {
        public Guid Id { get; init; }
        public string Body { get; init; }
        public DateTime DateCommented { get; init; }
        public DateTime? DateEdited { get; init; }

        public Guid AuthorId { get; init; }
        public string AuthorAvatar { get; init; }
        public Guid PostId { get; init; }
        public Guid? ReplyTo { get; init; }
    }
}