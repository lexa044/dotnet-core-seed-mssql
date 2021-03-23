using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using log4net;

using DNSeed.Security;
using DNSeed.Services;
using DNSeed.Models.Query;
using DNSeed.Resources;
using DNSeed.Models.Command;

namespace DNSeed.Controllers.V1
{
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILog _logger;
        private readonly IWebWorkContext _workContext;

        public ProductsController(
            ILog logger,
            IProductService productService,
            IWebWorkContext workContext)
        {
            _logger = logger;
            _productService = productService;
            _workContext = workContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = new ListResponse<ProductResponseModel>();
            try
            {
                response.Data = await _productService.GetAllAsync();
            }
            catch (Exception ex)
            {
                response.Meta.Code = -1;
                response.Meta.ErrorMessage = "Internal server error.";
                _logger.Error("There was an error on 'GetAllProducts' invocation.", ex);
            }

            return response.ToHttpResponse();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = new SingleResponse<ProductResponseModel>();
            try
            {
                response.Data = await _productService.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                response.Meta.Code = -1;
                response.Meta.ErrorMessage = "Internal server error.";
                _logger.Error("There was an error on 'GetProductById' invocation.", ex);
            }

            return response.ToHttpResponse();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SaveProductById(int id, [FromBody] ProductRequestModel request)
        {
            var response = new SingleResponse<ProductResponseModel>();
            try
            {
                request.Id = id;
                response.Data = await _productService.SaveAsync(request);
            }
            catch (Exception ex)
            {
                response.Meta.Code = -1;
                response.Meta.ErrorMessage = "Internal server error.";
                _logger.Error("There was an error on 'SaveProductById' invocation.", ex);
            }

            return response.ToHttpResponse();
        }

        [HttpGet("Paged")]
        public async Task<IActionResult> GetProductsPaged([FromQuery] PagedRequestModel filter)
        {
            var response = new PagedResponse<ProductResponseModel>();
            try
            {
                var result = await _productService.GetPagedAsync(filter);
                response.Data = result.Items;
                response.ItemsCount = result.TotalCount;
            }
            catch (Exception ex)
            {
                response.Meta.Code = -1;
                response.Meta.ErrorMessage = "Internal server error.";
                _logger.Error("There was an error on 'GetProductsPaged' invocation.", ex);
            }

            return response.ToHttpResponse();
        }
    }
}
