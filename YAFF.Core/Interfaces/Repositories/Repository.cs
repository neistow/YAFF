using System.Data;

namespace YAFF.Core.Interfaces.Repositories
{
    public abstract class Repository
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection => Transaction.Connection;

        public Repository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}