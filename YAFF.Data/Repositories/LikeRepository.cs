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

        public Task<int> AddLikeAsync(Guid postId, Guid userId)
        {
            var addLike = @"insert into postlikes (postid, userid)
                            values (@postId,@userId)";
            return Connection.ExecuteAsync(addLike, new {postId, userId});
        }

        public Task<int> RemoveLikeAsync(Guid postId, Guid userId)
        {
            var removeLike = @"delete from postlikes
                               where postid = @postId and userid = @userid";
            return Connection.ExecuteAsync(removeLike, new {postId, userId});
        }
    }
}