using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Core.Interfaces.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}