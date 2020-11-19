using System;

namespace YAFF.Data.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public Guid? ThumbnailId { get; set; }
        public Photo Thumbnail { get; set; }
    }
}