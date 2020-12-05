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
    public class TagRepository : Repository, ITagRepository
    {
        public TagRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<Tag> GetTagAsync(Guid id)
        {
            var sql = @"select t.id as TagId, t.name
                        from tags t 
                        where t.id = @id
                        limit 1";
            return await Connection.QuerySingleOrDefaultAsync<Tag>(sql, new {id});
        }

        public async Task<int> AddPostTagAsync(PostTag postTag)
        {
            var sql = @"insert into posttags(postid, tagid) 
                        values (@postId,@tagId)";
            return await Connection.ExecuteAsync(sql, new {postId = postTag.PostId, tagId = postTag.TagId});
        }

        public async Task<int> DeletePostTagAsync(PostTag postTag)
        {
            var sql = @"delete 
                        from posttags pt
                        where pt.postid = @postid and pt.tagid = @tagid";
            return await Connection.ExecuteAsync(sql, new {postTag.PostId, postTag.TagId});
        }

        public async Task UpdatePostTagsAsync(Guid postId, IEnumerable<PostTag> postTags)
        {
            var sql = @"select t.id as TagId, t.name
                        from posttags pt
                                 left join tags t on pt.tagid = t.id
                        where pt.postid = @id";
            var oldTags = (await Connection.QueryAsync<Tag>(sql, new {id = postId})).ToList();

            foreach (var postTag in postTags)
            {
                var index = oldTags.FindIndex(t => t.TagId == postTag.TagId);
                if (index == -1)
                {
                    await AddPostTagAsync(postTag);
                }
                else
                {
                    oldTags.RemoveAt(index);
                }
            }

            oldTags.ForEach(async t => await DeletePostTagAsync(new PostTag {PostId = postId, TagId = t.TagId}));
        }
    }
}