using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DNSeed.Repositories
{
    internal sealed class DalSession : IDalSession
    {
        private readonly string _readConnectionString;
        private readonly IUnitOfWork _uom;
        private SqlConnection _connection;

        public DalSession(string readConnectionString, string writeConnectionString)
        {
            _readConnectionString = readConnectionString ?? throw new ArgumentNullException(nameof(readConnectionString));
            _uom = new UnitOfWork(writeConnectionString);
        }

        public void Dispose()
        {
            if (null != _connection)
                _connection.Dispose();

            if (null != _uom)
                _uom.Dispose();

            _connection = null;
        }

        public async Task<DbConnection> GetReadOnlyConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_readConnectionString);
                await _connection.OpenAsync(cancellationToken);
            }
            return _connection;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return _uom;
        }
    }
}
