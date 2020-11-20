using System;

namespace YAFF.Core.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpires { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}