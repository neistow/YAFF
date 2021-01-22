namespace YAFF.Api.DTO.Comment
{
    public record UpdateCommentDto
    {
        public int Id { get; init; }
        public string Body { get; init; }
    }
}