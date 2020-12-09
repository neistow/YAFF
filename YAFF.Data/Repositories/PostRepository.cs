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

        public async Task<Post> GetPostAsync(Guid id)
        {
            var sql = @"select *,
                               (select count(*) from postlikes where postid = @id) as LikesCount
                        from posts p
                        where p.id = @id
                        limit 1;
                        
                        select *
                        from postlikes pl
                        where postid = @id;

                        select t.id, t.name
                        from posttags pt
                                 left join tags t on pt.tagid = t.id
                        where pt.postid = @id;";
            using var reader = await Connection.QueryMultipleAsync(sql, new {id});

            var post = await reader.ReadSingleOrDefaultAsync<Post>();
            var postLikes = await reader.ReadAsync<PostLike>();
            var tags = await reader.ReadAsync<Tag>();

            post?.PostLikes.AddRange(postLikes);
            post?.Tags.AddRange(tags);

            return post;
        }

        public async Task<List<Post>> GetPostsAsync(int page, int pageSize)
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
            return posts.Values.ToList();
        }

        public async Task<int> AddPostAsync(Post post)
        {
            var sql = @"insert into posts (Id, title, body, dateposted, authorid)
                        values (@id,@title,@body,@dateposted,@authorid)";

            return await Connection.ExecuteAsync(sql,
                new {post.Id, post.Title, post.Body, post.DatePosted, post.AuthorId});
        }

        public async Task<int> UpdatePostAsync(Post post)
        {
            var sql = @"update posts p set 
                        title = @title,
                        body = @body,
                        dateedited = @dateedited
                        where p.id = @id";

            return await Connection.ExecuteAsync(sql, new {post.Id, post.Title, post.Body, post.DateEdited});
        }

        public async Task<int> DeletePostAsync(Guid id)
        {
            var sql = @"delete 
                        from posts p 
                        where p.id = @id";
            return await Connection.ExecuteAsync(sql, new {id});
        }
    }
}