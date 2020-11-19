using System;
using System.Collections.Generic;

namespace YAFF.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? BanLiftDate { get; set; }
        public string PasswordHash { get; set; }

        public Guid? AvatarId { get; set; }
        public Photo Avatar { get; set; }

        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<PostComment> PostComments { get; set; }
        public IEnumerable<PostLike> LikedPosts { get; set; }
    }
}