using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class CommentRepository : Repository, ICommentRepository
    {
        public CommentRepository(IDbConnection connection) : base(connection)
        {
        }

        public Task<PostComment> GetCommentAsync(Guid id)
        {
            var sql = @"select * 
                        from postcomments pc
                        where pc.id = @id";
            return Connection.QuerySingleOrDefaultAsync<PostComment>(sql, new {id});
        }

        public Task<IEnumerable<PostComment>> GetCommentsOfPostAsync(Guid postId, int page, int pageSize)
        {
            var sql = @"select pc.id,
                        pc.postid,
                        pc.authorid,
                        pc.body,
                        pc.datecommented,
                        pc.dateedited,
                        pc.replyto,
                        p.id,
                        p.filename,
                        p.thumbnailid
                 from postcomments pc
                          left join users u on pc.authorid = u.id
                          left join photos p on u.avatarid = p.id
                 where pc.postid = @postid
                 order by pc.datecommented
                 offset @shift limit @pageSize";

            return Connection.QueryAsync<PostComment, Photo, PostComment>(sql,
                (comment, commenterAvatar) =>
                {
                    comment.Author = new User {Id = comment.AuthorId, Avatar = commenterAvatar};
                    return comment;
                },
                param: new {postId, shift = (page - 1) * pageSize, pageSize});
        }

        public Task<int> AddCommentAsync(PostComment comment)
        {
            var sql = @"insert into postcomments (id, postid, authorid, body, datecommented, dateedited, replyto)
                        values (@id,@postid,@authorid,@body,@datecommented,@dateedited,@replyto)";
            return Connection.ExecuteAsync(sql, new
            {
                comment.Id,
                comment.PostId,
                comment.AuthorId,
                comment.Body,
                comment.DateCommented,
                comment.DateEdited,
                comment.ReplyTo
            });
        }
    }
}