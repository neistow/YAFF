using System;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<int> AddAsync(RefreshToken entity);
        Task<int> DeleteAsync(Guid id);
        Task<RefreshToken> FindToken(Guid userId, string tokenString);
    }
}