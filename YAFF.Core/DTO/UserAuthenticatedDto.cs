namespace YAFF.Core.DTO
{
    public record UserAuthenticatedDto
    {
        public UserInfo User { get; init; }
        public string JwtToken { get; init; }
        public string RefreshToken { get; init; }
    }
}