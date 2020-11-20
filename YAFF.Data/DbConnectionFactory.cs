using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetSection("Data:ConnectionString").Value);
        }
    }
}