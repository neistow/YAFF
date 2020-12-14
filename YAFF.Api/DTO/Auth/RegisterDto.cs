namespace YAFF.Api.DTO.Auth
{
    public record RegisterDto
    {
        public string Nickname { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}