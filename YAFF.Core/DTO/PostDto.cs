using System;
using System.Collections.Generic;
using System.Linq;

namespace YAFF.Core.DTO
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? DateEdited { get; set; }
        public int LikesCount => PostLikes.Count();

        public Guid AuthorId { get; set; }

        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public IEnumerable<Guid> PostLikes { get; set; } = new List<Guid>();
        public IEnumerable<PostCommentDto> PostComments { get; set; } = new List<PostCommentDto>();
    }
}