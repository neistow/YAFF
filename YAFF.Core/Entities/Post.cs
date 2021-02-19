using System;
using System.Collections.Generic;
using System.Linq;
using YAFF.Core.Entities.Identity;

namespace YAFF.Core.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateEdited { get; set; }
        public int LikesCount => PostLikes.Count;

        public User Author { get; set; }
        public int AuthorId { get; set; }

        public PostPreview Preview { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<Comment> PostComments { get; set; } = new List<Comment>();
    }
}