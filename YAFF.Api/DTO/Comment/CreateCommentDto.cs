using System;

namespace YAFF.Api.DTO.Comment
{
    public record CreateCommentDto
    {
        public Guid PostId { get; init; }
        public string Body { get; init; }
        public Guid? ReplyTo { get; init; }
    }
}