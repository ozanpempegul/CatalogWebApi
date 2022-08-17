using CatalogWebApi.Base;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {        
        Task<(IEnumerable<Product> records, int total)> GetPaginationAsync(QueryResource pagination, ProductDto filterResource);
        Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId);
        void RemoveAsync(Product product);
        Task<bool> GetIsOfferable(int id);
        Task<IEnumerable<Product>> GetAllMyProductsAsync(int userId);
        void SellAsync(Product product);
    }
}
