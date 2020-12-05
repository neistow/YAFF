using System;
using System.Collections.Generic;

namespace YAFF.Core.Entities
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

        public List<Role> Roles { get; set; } = new List<Role>();
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<PostComment> PostComments { get; set; } = new List<PostComment>();
        public IEnumerable<PostLike> LikedPosts { get; set; } = new List<PostLike>();
        public IEnumerable<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}