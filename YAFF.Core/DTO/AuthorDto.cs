namespace YAFF.Core.DTO
{
    public record AuthorDto
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public string Avatar { get; init; }
    }
}