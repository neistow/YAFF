using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<PostComment>> GetCommentsOfPostAsync(Guid postId, int page, int pageSize);
    }
}