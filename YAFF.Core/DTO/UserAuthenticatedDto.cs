namespace YAFF.Core.DTO
{
    public class UserAuthenticatedDto
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}