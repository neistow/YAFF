using System;

namespace YAFF.Core.Entities
{
    public record RefreshToken
    {
        public Guid Id { get; init; }
        public string Token { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateExpires { get; init; }

        public Guid UserId { get; init; }
        public User User { get; init; }
    }
}