using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class LikeRepository : Repository, ILikeRepository
    {
        public LikeRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<int> AddLikeAsync(Guid postId, Guid userId)
        {
            var sql = @"insert into postlikes (postid, userid)
                        values (@postId,@userId)";
            return await Connection.ExecuteAsync(sql, new {postId, userId});
        }

        public async Task<int> RemoveLikeAsync(Guid postId, Guid userId)
        {
            var sql = @"delete from postlikes
                        where postid = @postId and userid = @userid";
            return await Connection.ExecuteAsync(sql, new {postId, userId});
        }
    }
}