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
    public class RefreshTokenRepository : Repository, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IDbConnection connection) : base(connection)
        {
        }

        public Task<int> AddTokenAsync(RefreshToken entity)
        {
            var addToken = @"insert into refreshtokens (id, token, datecreated, dateexpires, userid)
                             values (@id, @token, @datecreated, @dateexpires, @userid)";
            return Connection.ExecuteAsync(addToken, new
            {
                entity.UserId,
                entity.Id,
                entity.Token,
                entity.DateCreated,
                entity.DateExpires
            });
        }

        public Task<int> DeleteTokenAsync(Guid id)
        {
            var removeToken = @"delete from refreshtokens t where t.id = @id";
            return Connection.ExecuteAsync(removeToken, new {id});
        }

        public Task<RefreshToken> FindTokenAsync(Guid userId, string tokenString)
        {
            var findToken = @"select *
                              from refreshtokens t
                              where t.userid = @id
                                and t.token = @token
                              limit 1;";
            return Connection.QuerySingleOrDefaultAsync<RefreshToken>(findToken,
                new {id = userId, token = tokenString});
        }
    }
}