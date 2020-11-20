using System;
using System.Collections.Generic;

namespace YAFF.Core.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? DateEdited { get; set; }
        public int LikesCount { get; set; }
        
        public Guid AuthorId { get; set; }
        public User User { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<PostComment> PostComments { get; set; }
    }
}