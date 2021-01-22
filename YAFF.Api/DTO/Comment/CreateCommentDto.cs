namespace YAFF.Api.DTO.Comment
{
    public record CreateCommentDto
    {
        public int PostId { get; init; }
        public string Body { get; init; }
        public int? ReplyTo { get; init; }
    }
}