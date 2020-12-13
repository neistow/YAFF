using System;

namespace YAFF.Api.DTO.Comment
{
    public class CreateCommentDto
    {
        public Guid PostId { get; set; }
        public string Body { get; set; }
        public Guid? ReplyTo { get; set; }
    }
}