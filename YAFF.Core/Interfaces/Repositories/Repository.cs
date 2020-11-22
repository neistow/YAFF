using System.Data;

namespace YAFF.Core.Interfaces.Repositories
{
    public abstract class Repository
    {
        protected IDbConnection Connection { get; }

        protected Repository(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}