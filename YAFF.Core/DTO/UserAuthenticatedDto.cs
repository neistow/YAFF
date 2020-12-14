namespace YAFF.Core.DTO
{
    public record UserAuthenticatedDto
    {
        public string JwtToken { get; init; }
        public string RefreshToken { get; init; }
    }
}