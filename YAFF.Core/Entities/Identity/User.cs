using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace YAFF.Core.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public bool IsBanned { get; set; }
        public DateTime? BanLiftDate { get; set; }

        public UserProfile Profile { get; set; }

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<Comment> PostComments { get; set; } = new List<Comment>();
        public IEnumerable<PostLike> LikedPosts { get; set; } = new List<PostLike>();
    }
}