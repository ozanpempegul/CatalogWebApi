using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public interface IProductService : IBaseService<ProductDto, Product>
    {
        Task<PaginationResponse<IEnumerable<ProductDto>>> GetPaginationAsync(QueryResource pagination, ProductDto filterResource);
    }
}
