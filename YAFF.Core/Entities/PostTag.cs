using System;

namespace YAFF.Core.Entities
{
    public class PostTag
    {
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
        
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}