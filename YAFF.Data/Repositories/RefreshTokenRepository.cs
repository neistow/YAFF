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
        
        public async Task<int> AddAsync(RefreshToken entity)
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

        public async Task<int> DeleteAsync(Guid id)
        {
            var sql = @"delete from refreshtokens t where t.id = @id";
            return await Connection.ExecuteAsync(sql, new {id});
        }

        public async Task<RefreshToken> FindToken(Guid userId, string tokenString)
        {
            var sql = @"select * from find_refresh_token(@id,@token)";
            return await Connection.QuerySingleOrDefaultAsync<RefreshToken>(sql,
                new {id = userId, token = tokenString});
        }
    }
}