using System.Collections.Generic;
using System.Threading.Tasks;

using DNSeed.Domain;

namespace DNSeed.Repositories
{
    public interface IProductRepository
    {
        Task<Product> SaveAsync(Product request);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<PagedResponse<Product>> GetPagedAsync(PagedRequest request);
    }
}
