using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> GetTagAsync(Guid id);
        Task<int> AddPostTagAsync(PostTag postTag);
        Task<int> DeletePostTagAsync(PostTag postTag);
        Task UpdatePostTagsAsync(Guid postId, IEnumerable<PostTag> postTags);
    }
}