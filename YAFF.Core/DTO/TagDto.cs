using System;

namespace YAFF.Core.DTO
{
    public record TagDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}