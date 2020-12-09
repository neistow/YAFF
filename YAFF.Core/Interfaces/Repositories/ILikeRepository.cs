using System;
using System.Threading.Tasks;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<int> AddLikeAsync(Guid postId, Guid userId);
        Task<int> RemoveLikeAsync(Guid postId, Guid userId);
    }
}