namespace YAFF.Api.DTO.Auth
{
    public record RegisterDto
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}