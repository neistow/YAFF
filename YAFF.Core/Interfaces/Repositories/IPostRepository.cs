using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPost(Guid id);
        Task<IEnumerable<Post>> GetPosts(int page, int pageSize);
    }
}