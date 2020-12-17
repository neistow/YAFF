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
            var getPostWithUserQuery = @"select p.id,
                                                p.title,
                                                p.body,
                                                p.dateposted,
                                                p.dateedited,
                                                p.authorid,
                                                (select count(*) from postlikes where postid = @postId) as LikesCount,
                                                u.id,
                                                u.nickname,
                                                ph.id,
                                                ph.filename,
                                                ph.thumbnailid
                                         from posts p
                                                  left join users u on u.id = p.authorid
                                                  left join photos ph on ph.id = u.avatarid
                                         where p.id = @postId
                                         limit 1;";
            var getPostLikesAndTags = @"select *
                                        from postlikes pl
                                        where postid = @id;

                                        select t.id, t.name
                                        from posttags pt
                                                 left join tags t on pt.tagid = t.id
                                        where pt.postid = @id;";

            var post = (await Connection.QueryAsync<Post, User, Photo, Post>(getPostWithUserQuery,
                (p, u, a) => p with {User = u with {Avatar = a}},
                new {postId = id})).FirstOrDefault();
            if (post == null)
            {
                return null;
            }

            using var reader = await Connection.QueryMultipleAsync(getPostLikesAndTags, new {id});
            var postLikes = await reader.ReadAsync<PostLike>();
            var tags = await reader.ReadAsync<Tag>();

            return post with {PostLikes = postLikes, Tags = tags.ToList()};
        }

        public Task<IEnumerable<Post>> GetPostsAsync(int page, int pageSize)
        {
            var getPostsWithTags = @"select p.id,
                                            p.title,
                                            p.body,
                                            p.dateposted,
                                            p.dateedited,
                                            p.authorid,
                                            (select count(*) from postlikes where postid = p.id) as LikesCount,
                                            u.id, 
                                            u.nickname,
                                            ph.id, 
                                            ph.filename,
                                            ph.thumbnailid
                                     from (select * from posts offset @shift limit @pageSize) p
                                              left join users u on p.authorid = u.id 
                                              left join photos ph on ph.id = u.avatarid
                                     order by p.dateposted";

            return Connection.QueryAsync<Post, User, Photo, Post>(getPostsWithTags,
                (post, user, photo) => post with {User = user with {Avatar = photo}},
                new {shift = (page - 1) * pageSize, pageSize});
        }

        public Task<int> AddPostAsync(Post post)
        {
            var addPost = @"insert into posts (Id, title, body, dateposted, authorid)
                            values (@id,@title,@body,@dateposted,@authorid)";

            return Connection.ExecuteAsync(addPost,
                new {post.Id, post.Title, post.Body, post.DatePosted, post.AuthorId});
        }

        public Task<int> UpdatePostAsync(Post post)
        {
            var updatePost = @"update posts p set 
                               title = @title,
                               body = @body,
                               dateedited = @dateedited
                               where p.id = @id";

            return Connection.ExecuteAsync(updatePost, new {post.Id, post.Title, post.Body, post.DateEdited});
        }

        public Task<int> DeletePostAsync(Guid id)
        {
            var deletePost = @"delete 
                               from posts p 
                               where p.id = @id";
            return Connection.ExecuteAsync(deletePost, new {id});
        }
    }
}