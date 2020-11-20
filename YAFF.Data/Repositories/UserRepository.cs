using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }


        public override async Task<User> GetByIdAsync(Guid id)
        {
            var sql = @"select * from get_user_by_id(@id)";
            using var connection = ConnectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new {id});
        }

        public override Task<IReadOnlyList<User>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public override async Task<int> AddAsync(User entity)
        {
            var sql =
                @"insert into Users (Id, Nickname, RegistrationDate, Email, PasswordHash)
                  values (@id, @nickname, @registrationdate, @email, @passwordhash);";
            using var connection = ConnectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new
            {
                entity.Id,
                entity.NickName,
                entity.Email,
                entity.PasswordHash,
                entity.RegistrationDate
            });
        }

        public override Task<int> UpdateAsync(User entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<int> DeleteAsync(Guid id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var sql = @"select * from get_user_by_email(@email)";
            using var connection = ConnectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new {email});
        }
    }
}