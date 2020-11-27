using System;

namespace YAFF.Core.DTO
{
    public class PostCommentDto
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime DateCommented { get; set; }
        public DateTime? DateEdited { get; set; }
        
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid? ReplyTo { get; set; }
    }
}