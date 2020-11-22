using System;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Core.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
    }
}