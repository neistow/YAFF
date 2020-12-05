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

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            var sql1 = @"select r.id, r.name
                         from userroles ur
                                  join roles r on ur.roleid = r.id
                         where ur.userid = @userId";
            var roles = await Connection.QueryAsync<Role>(sql1, new {userId});

            return roles.Where(r => r.Id != Guid.Empty).ToArray();
        }
    }
}