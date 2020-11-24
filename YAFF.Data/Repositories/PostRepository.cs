using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class PostRepository : Repository, IPostRepository
    {
        public PostRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<Post>> GetPosts(int page, int pageSize)
        {
            var sql = @"select p.id,
                               p.title,
                               p.body,
                               p.dateposted,
                               p.dateedited,
                               p.authorid,
                               (select count(*) from postlikes where postid = p.id) as LikesCount,
                               pt.tagid,
                               t.name
                        from (select * from posts offset @shift limit @pageSize) p
                                 left join posttags pt on pt.postid = p.id
                                 left join tags t on pt.tagid = t.id
                        order by p.dateposted";

            var posts = new Dictionary<Guid, Post>();
            await Connection.QueryAsync<Post, Tag, Post>(sql, (post, tag) =>
            {
                if (!posts.TryGetValue(post.Id, out var entry))
                {
                    entry = post;
                    posts.Add(entry.Id, entry);
                }

                if (tag != null)
                {
                    entry.Tags.Add(tag);
                }

                return entry;
            }, splitOn: "TagId", param: new {shift = (page - 1) * pageSize, pageSize});
            return posts.Values.ToArray();
        }
    }
}