using System;

namespace YAFF.Core.Entities
{
    public record PostLike
    {
        public Guid PostId { get; init; }
        public Post Post { get; init; }

        public Guid UserId { get; init; }
        public User User { get; init; }
    }
}