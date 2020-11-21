using System;
using System.Data;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;
using YAFF.Data.Repositories;

namespace YAFF.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IRefreshTokenRepository _tokenRepository;


        public UnitOfWork(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_transaction);
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_transaction);

        public IRefreshTokenRepository RefreshTokenRepository =>
            _tokenRepository ??= new RefreshTokenRepository(_transaction);

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _roleRepository = null;
            _tokenRepository = null;
            _userRepository = null;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }

            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}