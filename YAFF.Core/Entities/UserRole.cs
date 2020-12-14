using System;

namespace YAFF.Core.Entities
{
    public record UserRole
    {
        public Guid RoleId { get; init; }
        public Role Role { get; init; }

        public Guid UserId { get; init; }
        public User User { get; init; }
    }
}