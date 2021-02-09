namespace YAFF.Core.DTO
{
    public record PostPreviewDto
    {
        public int Id { get; init; }
        public string Body { get; init; }
        public string Image { get; init; }

        public int PostId { get; init; }
    }
}