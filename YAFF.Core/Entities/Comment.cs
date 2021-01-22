using System;
using YAFF.Core.Entities.Identity;

namespace YAFF.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateEdited { get; set; }
        
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? ReplyToId { get; set; }
        public Comment ReplyTo { get; set; }
    }
}