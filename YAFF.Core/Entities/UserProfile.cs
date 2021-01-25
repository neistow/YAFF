using System;
using YAFF.Core.Entities.Identity;

namespace YAFF.Core.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string UserStatus { get; set; }
        public string Bio { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public UserProfileType ProfileType { get; set; } = UserProfileType.Public;

        
        public int? AvatarId { get; set; }
        public Photo Avatar { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}