using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using YAFF.Core.Entities;
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
            var sql = @"drop table if exists ""user"";
                        create temp table ""user"" as
                        select *
                        from users u
                        where u.id = @id
                        limit 1;

                        select *
                        from ""user"";

                        select r.id, r.name
                        from userroles ur
                                 join roles r on ur.roleid = r.id
                        where ur.userid = @id;

                        select *
                        from photos p
                        where p.id = (select u.id from ""user"" u);
                        drop table if exists ""user""";
            using var reader = await Connection.QueryMultipleAsync(sql, new {id});
            var user = await reader.ReadSingleOrDefaultAsync<User>();
            var roles = await reader.ReadAsync<Role>();
            var avatar = await reader.ReadSingleOrDefaultAsync<Photo>();

            user?.Roles.AddRange(roles);
            if (user != null)
            {
                user.Avatar = avatar;
            }
            return user;
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            var sql = @"select *
                         from users u
                         where u.email = @email
                         limit 1";
            return Connection.QuerySingleOrDefaultAsync<User>(sql, new {email});
        }

        public Task<int> AddUserAsync(User entity)
        {
            var sql = @"insert into users (id, nickname, registrationdate, email, passwordhash)
                        values (@id, @nickname, @registrationdate, @email, @passwordhash);";
            return Connection.ExecuteAsync(sql, new
            {
                entity.Id,
                entity.NickName,
                entity.RegistrationDate,
                entity.Email,
                entity.PasswordHash
            });
        }
    }
}