using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<PostComment> GetCommentAsync(Guid id);
        Task<IEnumerable<PostComment>> GetCommentsOfPostAsync(Guid postId, int page, int pageSize);
        Task<int> AddCommentAsync(PostComment comment);
        Task<int> UpdateCommentAsync(PostComment comment);
        Task<int> DeleteCommentAsync(Guid id);
    }
}