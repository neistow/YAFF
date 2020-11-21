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
        public UserRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var sql1 = @"select * from get_user_by_id(@id)";
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
            var sql1 = @"select * from get_user_by_email(@email)";
            return await Connection.QuerySingleOrDefaultAsync<User>(sql1, new {email});
        }
    }
}