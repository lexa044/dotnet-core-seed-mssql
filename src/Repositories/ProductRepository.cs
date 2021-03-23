using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using DNSeed.Domain;

namespace DNSeed.Repositories
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly IDalSession _session;

        public ProductRepository(IDalSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var connection = await _session.GetReadOnlyConnectionAsync();
            string sql = @"SELECT TOP 100 * FROM dbo.Product;";
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var connection = await _session.GetReadOnlyConnectionAsync();
            string sql = @"SELECT TOP 1 * FROM dbo.Product WHERE [Id]=@id;";
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { id});
        }

        public async Task<Product> SaveAsync(Product request)
        {
            CheckEmptyValues(request);
            var uom = _session.GetUnitOfWork();
            if (request.Id == 0)
            {
                return await AddAsync(request, uom);
            }
            else
            {
                return await UpdateAsync(request, uom);
            }
        }

        public async Task<PagedResponse<Product>> GetPagedAsync(PagedRequest request)
        {
            var results = new PagedResponse<Product>();
            var connection = await _session.GetReadOnlyConnectionAsync();
            var countQuery = "SELECT COUNT(1) FROM dbo.Product f WHERE (@Id = 0 OR f.[Id] = @Id);";
            if (string.IsNullOrEmpty(request.OrderBy))
                request.OrderBy = "Id DESC";
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT f.* FROM dbo.Product f WHERE (@Id = 0 OR f.[Id] = @Id) ");
            string[] tokens = request.OrderBy.ToLower().Split(' ');
            builder.Append(" ORDER BY f.[").Append(tokens[0]).Append("]").Append(" ").Append(tokens[1]);
            builder.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;");
            builder.Append(countQuery);

            using (var multi = await connection.QueryMultipleAsync(builder.ToString(),
                        new
                        {
                            Id = request.AggregateId,
                            Offset = request.Page * request.PageSize,
                            PageSize = request.PageSize,
                            //From = request.FromDate,
                            //To = request.ToDate
                        }))
            {
                results.Items = new List<Product>(await multi.ReadAsync<Product>());
                results.TotalCount = await multi.ReadFirstAsync<int>();
            }

            return results;
        }

        private async Task<Product> AddAsync(Product request, IUnitOfWork uom)
        {
            string insertSQL = @"INSERT INTO dbo.Product ([SKU],[Name],[ImageUri],[Status],[CreatedDate])
                        VALUES (@SKU,@Name,@ImageUri,@Status,@CreatedDate);
                        SELECT SCOPE_IDENTITY();";
            request.CreatedDate = DateTime.Now;
            var connection = await uom.GetConnectionAsync();
            request.Id = await connection.ExecuteScalarAsync<int>(insertSQL, new
            {
                SKU = request.SKU,
                Name = request.Name,
                ImageUri = request.ImageUri,
                Status = request.Status,
                CreatedDate = request.CreatedDate
            }, uom.GetTransaction());

            return request;
        }

        private async Task<Product> UpdateAsync(Product request, IUnitOfWork uom)
        {
            string updateSQL = @"UPDATE dbo.Product SET [SKU]=@SKU,[Name]=@Name,[ImageUri]=@ImageUri,[Status]=@Status,[UpdatedDate]=@UpdatedDate WHERE [Id]=@Id;";
            request.UpdatedDate = DateTime.Now;
            var connection = await uom.GetConnectionAsync();
            await connection.ExecuteScalarAsync<int>(updateSQL, new
            {
                SKU = request.SKU,
                Name = request.Name,
                ImageUri = request.ImageUri,
                Status = request.Status,
                UpdatedDate = request.UpdatedDate,
                Id= request.Id
            }, uom.GetTransaction());

            return request;
        }

        private void CheckEmptyValues(Product request)
        {
            if (string.IsNullOrEmpty(request.Name))
                request.Name = "";
            if (string.IsNullOrEmpty(request.ImageUri))
                request.ImageUri = "";
        }
    }
}
