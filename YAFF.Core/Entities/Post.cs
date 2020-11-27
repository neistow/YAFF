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
        
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public List<PostComment> PostComments { get; set; } = new List<PostComment>();
    }
}