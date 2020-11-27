using System;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> GetTag(Guid id);
        Task<int> AddPostTag(PostTag postTag);
    }
}