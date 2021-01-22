namespace YAFF.Core.DTO
{
    public record UserAuthenticatedDto
    {
        public UserDto User { get; init; }
        public string JwtToken { get; init; }
    }
}