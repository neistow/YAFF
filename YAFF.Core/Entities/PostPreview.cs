namespace YAFF.Core.Entities
{
    public class PostPreview
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public Photo Image { get; set; }

        public Post Post { get; set; }
        public int PostId { get; set; }
    }
}