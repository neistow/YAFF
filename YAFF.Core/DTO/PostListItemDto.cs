using System;
using System.Collections.Generic;

namespace YAFF.Core.DTO
{
    public class PostListItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? DateEdited { get; set; }
        public int LikesCount { get; set; }
        public Guid AuthorId { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
    }
}