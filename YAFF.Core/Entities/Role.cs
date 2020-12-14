using System;

namespace YAFF.Core.Entities
{
    public record Role
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}