using System.Collections.Generic;
using System.Threading.Tasks;

using DNSeed.Models.Command;
using DNSeed.Models.Query;

namespace DNSeed.Services
{
    public interface IProductService
    {
        Task<ProductResponseModel> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponseModel>> GetAllAsync();
        Task<PagedResponseModel<ProductResponseModel>> GetPagedAsync(PagedRequestModel request);
        Task<ProductResponseModel> SaveAsync(ProductRequestModel request);
    }
}
