using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(IDbConnection connection) : base(connection)
        {
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var sql1 = @"select *
                        from users u
                        where u.id = @id
                        limit 1";
            return await Connection.QuerySingleOrDefaultAsync<User>(sql1, new {id});
        }

        public async Task<int> AddAsync(User entity)
        {
            var sql = @"insert into users (id, nickname, registrationdate, email, passwordhash)
                        values (@id, @nickname, @registrationdate, @email, @passwordhash);";
            return await Connection.ExecuteAsync(sql, new
            {
                entity.Id,
                entity.NickName,
                entity.RegistrationDate,
                entity.Email,
                entity.PasswordHash
            });
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var sql = @"select *
                         from users u
                         where u.email = @email
                         limit 1";
            return await Connection.QuerySingleOrDefaultAsync<User>(sql, new {email});
        }
    }
}