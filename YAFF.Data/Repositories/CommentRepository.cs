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

        public Task<IEnumerable<PostComment>> GetCommentsOfPostAsync(Guid postId, int page, int pageSize)
        {
            var sql = @"select * from postcomments
                        where postid = @postid
                        order by datecommented desc
                        offset @shift limit @pageSize";
            return Connection.QueryAsync<PostComment>(sql, new {postId, shift = (page - 1) * pageSize, pageSize});
        }
    }
}