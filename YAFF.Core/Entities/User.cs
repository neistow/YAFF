using System;
using System.Collections.Generic;

namespace YAFF.Core.Entities
{
    public record User
    {
        public Guid Id { get; init; }
        public string NickName { get; init; }
        public DateTime RegistrationDate { get; init; }
        public string Email { get; init; }
        public bool EmailConfirmed { get; init; }
        public bool IsBanned { get; init; }
        public DateTime? BanLiftDate { get; init; }
        public string PasswordHash { get; init; }

        public Guid? AvatarId { get; init; }
        public Photo Avatar { get; init; }

        public IEnumerable<Role> Roles { get; init; } = new List<Role>();
        public IEnumerable<Post> Posts { get; init; } = new List<Post>();
        public IEnumerable<PostComment> PostComments { get; init; } = new List<PostComment>();
        public IEnumerable<PostLike> LikedPosts { get; init; } = new List<PostLike>();
        public IEnumerable<RefreshToken> RefreshTokens { get; init; } = new List<RefreshToken>();
    }
}