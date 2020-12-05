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

        public async Task<int> AddTokenAsync(RefreshToken entity)
        {
            var sql = @"insert into refreshtokens (id, token, datecreated, dateexpires, userid)
                        values (@id, @token, @datecreated, @dateexpires, @userid)";
            return await Connection.ExecuteAsync(sql, new
            {
                entity.UserId,
                entity.Id,
                entity.Token,
                entity.DateCreated,
                entity.DateExpires
            });
        }

        public async Task<int> DeleteTokenAsync(Guid id)
        {
            var sql = @"delete from refreshtokens t where t.id = @id";
            return await Connection.ExecuteAsync(sql, new {id});
        }

        public async Task<RefreshToken> FindTokenAsync(Guid userId, string tokenString)
        {
            var sql = @"select *
                        from refreshtokens t
                        where t.userid = @id
                          and t.token = @token
                        limit 1;";
            return await Connection.QuerySingleOrDefaultAsync<RefreshToken>(sql,
                new {id = userId, token = tokenString});
        }
    }
}