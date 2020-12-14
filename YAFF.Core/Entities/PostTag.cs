using System;

namespace YAFF.Core.Entities
{
    public record PostTag
    {
        public Guid TagId { get; init; }
        public Tag Tag { get; init; }
        
        public Guid PostId { get; init; }
        public Post Post { get; init; }
    }
}