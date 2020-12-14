using System;

namespace YAFF.Core.Entities
{
    public record Tag
    {
        public Guid TagId { get; init; }
        public string Name { get; init; }
    }
}