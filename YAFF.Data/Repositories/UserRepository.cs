using System;
using System.Data;
using System.Linq;
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
            var getUser = @"select *
                            from users u
                            where u.id = @id
                            limit 1";
            var getUserAvatarAndRoles = @"select r.id, r.name
                                          from userroles ur
                                                   join roles r on ur.roleid = r.id
                                          where ur.userid = @userId;
                                          
                                          select p.id, p.filename, p.thumbnailid
                                          from users u
                                                   join photos p on u.avatarid = p.id
                                          where u.id = @userId
                                          limit 1;";

            var user = await Connection.QuerySingleOrDefaultAsync<User>(getUser, new {id});
            if (user == null)
            {
                return null;
            }

            using var reader = await Connection.QueryMultipleAsync(getUserAvatarAndRoles, new {userId = user.Id});
            var roles = await reader.ReadAsync<Role>();
            var avatar = await reader.ReadSingleOrDefaultAsync<Photo>();

            return user with {Roles = roles.ToList(), Avatar = avatar};
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var getUser = @"select *
                            from users u
                            where u.email = @email
                            limit 1";

            var getUserAvatarAndRoles = @"select r.id, r.name
                                          from userroles ur
                                                   join roles r on ur.roleid = r.id
                                          where ur.userid = @userId;
                                          
                                          select p.id, p.filename, p.thumbnailid
                                          from users u
                                                   join photos p on u.avatarid = p.id
                                          where u.id = @userId
                                          limit 1;";

            var user = await Connection.QuerySingleOrDefaultAsync<User>(getUser, new {email});
            if (user == null)
            {
                return null;
            }

            using var reader = await Connection.QueryMultipleAsync(getUserAvatarAndRoles, new {userId = user?.Id});
            var roles = await reader.ReadAsync<Role>();
            var avatar = await reader.ReadSingleOrDefaultAsync<Photo>();

            return user with {Roles = roles.ToList(), Avatar = avatar};
        }

        public Task<int> AddUserAsync(User entity)
        {
            var addUser = @"insert into users (id, nickname, registrationdate, email, passwordhash)
                            values (@id, @nickname, @registrationdate, @email, @passwordhash);";
            return Connection.ExecuteAsync(addUser, new
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