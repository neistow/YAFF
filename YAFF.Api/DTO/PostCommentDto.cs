using System;

namespace YAFF.Api.DTO
{
    public class PostCommentDto
    {
        public string Body { get; set; }
        public Guid? ReplyTo { get; set; }
    }
}