using System;
using System.Threading.Tasks;
using YAFF.Core.Entities;

namespace YAFF.Core.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<int> AddTokenAsync(RefreshToken entity);
        Task<int> DeleteTokenAsync(Guid id);
        Task<RefreshToken> FindTokenAsync(Guid userId, string tokenString);
    }
}