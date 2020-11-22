using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class RoleRepository : Repository, IRoleRepository
    {
        public RoleRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<Role>> GetUserRoles(Guid userId)
        {
            var sql1 = @"select * from get_user_roles(@id)";
            var roles = await Connection.QueryAsync<Role>(sql1, new {id = userId});

            return roles.Where(r => r.Id != Guid.Empty).ToArray();
        }
    }
}