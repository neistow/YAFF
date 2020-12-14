namespace YAFF.Api.DTO.Auth
{
    public record RefreshTokenDto
    {
        public string Token { get; init; }
    }
}