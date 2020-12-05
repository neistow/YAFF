using System;
using System.Linq;
using YAFF.Core.Entities;

namespace YAFF.Core.Extensions
{
    public static class UserExtensions
    {
        private static bool IsAdmin(this User user)
        {
            return user.Roles.SingleOrDefault(r =>
                r.Name.Equals("admin", StringComparison.InvariantCultureIgnoreCase)) != null;
        }

        private static bool IsModerator(this User user)
        {
            return user.Roles.SingleOrDefault(r =>
                r.Name.Equals("moderator", StringComparison.InvariantCultureIgnoreCase)) != null;
        }

        public static bool CanDeletePosts(this User user)
        {
            var isAdmin = user.IsAdmin();
            var isModerator = user.IsModerator();

            return isAdmin || isModerator;
        }

        public static bool CanManagePosts(this User user)
        {
            return user.IsAdmin();
        }
    }
}