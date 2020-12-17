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

        public Task<Tag> GetTagAsync(Guid id)
        {
            var getTag = @"select t.id as TagId, t.name
                           from tags t 
                           where t.id = @id
                           limit 1";
            return Connection.QuerySingleOrDefaultAsync<Tag>(getTag, new {id});
        }

        public Task<int> AddPostTagAsync(PostTag postTag)
        {
            var addTag = @"insert into posttags(postid, tagid) 
                           values (@postId,@tagId)";
            return Connection.ExecuteAsync(addTag, new {postId = postTag.PostId, tagId = postTag.TagId});
        }

        public Task<int> DeletePostTagAsync(PostTag postTag)
        {
            var deleteTag = @"delete 
                              from posttags pt
                              where pt.postid = @postid and pt.tagid = @tagid";
            return Connection.ExecuteAsync(deleteTag, new {postTag.PostId, postTag.TagId});
        }

        public async Task UpdatePostTagsAsync(Guid postId, IEnumerable<PostTag> postTags)
        {
            var updateTags = @"select t.id as TagId, t.name
                               from posttags pt
                                        left join tags t on pt.tagid = t.id
                               where pt.postid = @id";

            var oldTags = (await Connection.QueryAsync<Tag>(updateTags, new {id = postId})).ToList();
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