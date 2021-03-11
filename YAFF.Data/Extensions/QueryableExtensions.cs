using System.Linq;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;

namespace YAFF.Data.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Includes Author with profile and avatar
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static IQueryable<Post> IncludeAuthor(this IQueryable<Post> posts)
        {
            return posts
                .Include(p => p.Author)
                .ThenInclude(u => u.Profile)
                .ThenInclude(p => p.Avatar);
        }

        /// <summary>
        /// Includes PostLikes
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static IQueryable<Post> IncludeLikes(this IQueryable<Post> posts)
        {
            return posts
                .Include(p => p.PostLikes);
        }

        /// <summary>
        /// Includes PostTags with Tags
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static IQueryable<Post> IncludeTags(this IQueryable<Post> posts)
        {
            return posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag);
        }

        /// <summary>
        /// Includes Preview of the post with photo
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static IQueryable<Post> IncludePreview(this IQueryable<Post> posts)
        {
            return posts
                .Include(p => p.Preview)
                .ThenInclude(pp => pp.Image);
        }

        /// <summary>
        /// Includes Author with profile and avatar
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        public static IQueryable<Comment> IncludeAuthor(this IQueryable<Comment> comments)
        {
            return comments
                .Include(c => c.Author)
                .ThenInclude(u => u.Profile)
                .ThenInclude(p => p.Avatar);
        }

        /// <summary>
        /// Includes user with avatar
        /// </summary>
        /// <param name="profiles"></param>
        /// <returns></returns>
        public static IQueryable<UserProfile> IncludeUser(this IQueryable<UserProfile> profiles)
        {
            return profiles
                .Include(p => p.User)
                .Include(p => p.Avatar);
        }

        /// <summary>
        /// Includes users of the chat with their profiles and avatars
        /// </summary>
        /// <param name="chats"></param>
        /// <returns></returns>
        public static IQueryable<Chat> IncludeUsersWithProfiles(this IQueryable<Chat> chats)
        {
            return chats.Include(c => c.Users)
                .ThenInclude(cu => cu.User)
                .ThenInclude(u => u.Profile)
                .ThenInclude(up => up.Avatar);
        }

        /// <summary>
        /// Includes user with profile and avatar
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static IQueryable<User> IncludeProfile(this IQueryable<User> users)
        {
            return users
                .Include(u => u.Profile)
                .ThenInclude(up => up.Avatar);
        }

        /// <summary>
        /// Performs a pagination on a generic collection
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> entities, int page, int pageSize)
        {
            return entities.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}