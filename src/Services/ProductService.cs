using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using DNSeed.Domain;
using DNSeed.Models.Command;
using DNSeed.Models.Query;
using DNSeed.Repositories;

namespace DNSeed.Services
{
    internal sealed class ProductService : IProductService
    {
        private readonly IDalSession _session;
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(
            IDalSession session,
            IProductRepository repository,
            IMapper mapper)
        {
            _session = session;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseModel>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductResponseModel>>(response);
        }

        public async Task<ProductResponseModel> GetByIdAsync(int id)
        {
            var response = await _repository.GetByIdAsync(id);
            return _mapper.Map<ProductResponseModel>(response);
        }

        public async Task<ProductResponseModel> SaveAsync(ProductRequestModel request)
        {
            var staleDomain = _mapper.Map<Product>(request);
            var currentDomain = await _repository.GetByIdAsync(request.Id);
            if (null != currentDomain)
            {
                staleDomain.CreatedDate = currentDomain.CreatedDate;
                staleDomain.UpdatedDate = currentDomain.UpdatedDate;
            }
            var response = await _repository.SaveAsync(staleDomain);
            _session.GetUnitOfWork().CommitChanges();
            return _mapper.Map<ProductResponseModel>(response);
        }

        public async Task<PagedResponseModel<ProductResponseModel>> GetPagedAsync(PagedRequestModel request)
        {
            var domain = _mapper.Map<PagedRequest>(request);
            var response = await _repository.GetPagedAsync(domain);

            return new PagedResponseModel<ProductResponseModel>()
            {
                Items = _mapper.Map<IEnumerable<ProductResponseModel>>(response.Items),
                TotalCount = response.TotalCount
            };
        }
    }
}
