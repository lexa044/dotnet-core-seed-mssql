using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DNSeed.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private readonly string _connectionString;

        public UnitOfWork(string connectionString) => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public async Task<DbConnection> GetConnectionAsync(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                await _connection.OpenAsync(cancellationToken);
                _transaction = (SqlTransaction)await _connection.BeginTransactionAsync();
            }

            return _connection;
        }

        public DbTransaction GetTransaction()
        {
            return _transaction;
        }

        public void CommitChanges()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _connection.Close();
            }
        }

        public void Dispose()
        {
            if (null != _transaction)
                _transaction.Dispose();

            if (null != _connection)
                _connection.Dispose();

            _transaction = null;
            _connection = null;
        }
    }
}
