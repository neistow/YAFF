namespace YAFF.Business.Common
{
    public static class PasswordHasher
    {
        public static bool VerifyPasswordHash(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}