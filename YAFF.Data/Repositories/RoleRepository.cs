using System.Data;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class RoleRepository : Repository, IRoleRepository
    {
        public RoleRepository(IDbConnection connection) : base(connection)
        {
        }
    }
}