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
            var getComments = @"select * 
                                from postcomments pc
                                where pc.id = @id";
            return Connection.QuerySingleOrDefaultAsync<PostComment>(getComments, new {id});
        }

        public Task<IEnumerable<PostComment>> GetCommentsOfPostAsync(Guid postId, int page, int pageSize)
        {
            var getComments = @"select pc.id,
                                       pc.postid,
                                       pc.authorid,
                                       pc.body,
                                       pc.dateadded,
                                       pc.dateedited,
                                       pc.replyto,
                                       u.id,
                                       u.nickname,
                                       p.id,
                                       p.filename,
                                       p.thumbnailid
                                from postcomments pc
                                         left join users u on pc.authorid = u.id
                                         left join photos p on u.avatarid = p.id
                                where pc.postid = @postid
                                order by pc.dateadded
                                offset @shift limit @pageSize";

            return Connection.QueryAsync<PostComment, User, Photo, PostComment>(getComments,
                (comment, author, authorAvatar) =>
                    comment with{Author = author with {Avatar = authorAvatar}},
                new {postId, shift = (page - 1) * pageSize, pageSize});
        }

        public Task<int> AddCommentAsync(PostComment comment)
        {
            var addComment = @"insert into postcomments (id, postid, authorid, body, dateadded, dateedited, replyto)
                               values (@id, @postid, @authorid, @body, @dateadded, @replyto)";
            return Connection.ExecuteAsync(addComment, new
            {
                comment.Id,
                comment.PostId,
                comment.AuthorId,
                comment.Body,
                DateCommented = comment.DateAdded,
                comment.ReplyTo
            });
        }

        public Task<int> UpdateCommentAsync(PostComment comment)
        {
            var updateComment = @"update postcomments pc
                                  set body = @body, dateedited = @dateedited
                                  where pc.id = @id";
            return Connection.ExecuteAsync(updateComment, new {comment.Body, comment.DateEdited, comment.Id});
        }

        public Task<int> DeleteCommentAsync(Guid id)
        {
            var deleteComment = @"delete from
                                  postcomments pc
                                  where pc.id = @id";
            return Connection.ExecuteAsync(deleteComment, new {id});
        }

        public Task<int> GetCommentsCountForPost(Guid postId)
        {
            var commentsCount = @"select count(*)
                                 from postcomments pc
                                 where pc.postid = @postId";
            return Connection.QuerySingleAsync<int>(commentsCount, new {postId});
        }
    }
}