using System;

namespace YAFF.Data.Entities
{
    public class PostTag
    {
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
        
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}