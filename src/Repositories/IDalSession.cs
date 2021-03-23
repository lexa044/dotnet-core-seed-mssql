using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DNSeed.Repositories
{
    public interface IDalSession : IDisposable
    {
        Task<DbConnection> GetReadOnlyConnectionAsync(CancellationToken cancellationToken = default);
        IUnitOfWork GetUnitOfWork();
    }
}
