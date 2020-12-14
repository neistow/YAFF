using System;

namespace YAFF.Core.Entities
{
    public record Photo
    {
        public Guid Id { get; init; }
        public string FileName { get; init; }
        public Guid? ThumbnailId { get; init; }
        public Photo Thumbnail { get; init; }
    }
}