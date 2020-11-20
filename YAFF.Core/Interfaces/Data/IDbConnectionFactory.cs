using System.Data;

namespace YAFF.Core.Interfaces.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}