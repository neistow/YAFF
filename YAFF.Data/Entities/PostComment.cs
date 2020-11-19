using System;

namespace YAFF.Data.Entities
{
    public class PostComment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime DateCommented { get; set; }
        public DateTime? DateEdited { get; set; }
        
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public Guid ReplyTo { get; set; }
        public PostComment Comment { get; set; }
    }
}