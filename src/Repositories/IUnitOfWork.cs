using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DNSeed.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<DbConnection> GetConnectionAsync(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
        DbTransaction GetTransaction();
        void CommitChanges();
    }
}
