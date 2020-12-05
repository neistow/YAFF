using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostAsync(Guid id);
        Task<List<Post>> GetPostsAsync(int page, int pageSize);
        Task<int> AddPostAsync(Post post);
        Task<int> UpdatePostAsync(Post post);
        Task<int> DeletePostAsync(Guid id);
    }
}