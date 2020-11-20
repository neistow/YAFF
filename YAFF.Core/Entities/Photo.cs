using System;

namespace YAFF.Core.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public Guid? ThumbnailId { get; set; }
        public Photo Thumbnail { get; set; }
    }
}