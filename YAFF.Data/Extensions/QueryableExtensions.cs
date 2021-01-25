using System.Linq;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Entities;

namespace YAFF.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Post> IncludeAuthor(this IQueryable<Post> posts)
        {
            return posts.Include(p => p.Author)
                .ThenInclude(u => u.Profile)
                .ThenInclude(p => p.Avatar);
        }

        public static IQueryable<Post> IncludeLikes(this IQueryable<Post> posts)
        {
            return posts.Include(p => p.PostLikes);
        }

        public static IQueryable<Post> IncludeTags(this IQueryable<Post> posts)
        {
            return posts.Include(p => p.PostTags).ThenInclude(pt => pt.Tag);
        }

        public static IQueryable<Comment> IncludeAuthor(this IQueryable<Comment> comments)
        {
            return comments.Include(c => c.Author)
                .ThenInclude(u => u.Profile)
                .ThenInclude(p => p.Avatar);
        }

        public static IQueryable<UserProfile> IncludeUser(this IQueryable<UserProfile> profiles)
        {
            return profiles.Include(p => p.User).Include(p => p.Avatar);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> entities, int page, int pageSize)
        {
            return entities.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}