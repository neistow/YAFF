using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Core.Interfaces.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnectionFactory ConnectionFactory;

        protected Repository(IDbConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public abstract Task<T> GetByIdAsync(Guid id);
        public abstract Task<IReadOnlyList<T>> GetAllAsync();
        public abstract Task<int> AddAsync(T entity);
        public abstract Task<int> UpdateAsync(T entity);
        public abstract Task<int> DeleteAsync(Guid id);
    }
}