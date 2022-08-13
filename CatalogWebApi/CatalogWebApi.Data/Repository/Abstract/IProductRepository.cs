using CatalogWebApi.Base;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<(IEnumerable<Product> records, int total)> GetPaginationAsync(QueryResource pagination, ProductDto filterResource);
    }
}
