using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.Entities;

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
            return posts.Include(p => p.Author)
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
            return posts.Include(p => p.PostLikes);
        }

        /// <summary>
        /// Includes PostTags with Tags
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static IQueryable<Post> IncludeTags(this IQueryable<Post> posts)
        {
            return posts.Include(p => p.PostTags).ThenInclude(pt => pt.Tag);
        }

        /// <summary>
        /// Includes Author with profile and avatar
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        public static IQueryable<Comment> IncludeAuthor(this IQueryable<Comment> comments)
        {
            return comments.Include(c => c.Author)
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
            return profiles.Include(p => p.User).Include(p => p.Avatar);
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