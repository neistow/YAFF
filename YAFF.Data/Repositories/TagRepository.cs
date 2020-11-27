using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class TagRepository : Repository, ITagRepository
    {
        public TagRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<Tag> GetTag(Guid id)
        {
            var sql = @"select t.id as TagId, t.name
                        from tags t 
                        where t.id = @id
                        limit 1";
            return await Connection.QuerySingleOrDefaultAsync<Tag>(sql, new {id});
        }

        public async Task<int> AddPostTag(PostTag postTag)
        {
            var sql = @"insert into posttags(postid, tagid) 
                        values (@postId,@tagId)";
            return await Connection.ExecuteAsync(sql, new {postId = postTag.PostId, tagId = postTag.TagId});
        }
    }
}