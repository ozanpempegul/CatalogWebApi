using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using Microsoft.AspNetCore.Http;

namespace CatalogWebApi.Service
{
    public interface IProductService : IBaseService<ProductDto, Product>
    {
        Task<BaseResponse<ProductDto>> InsertAsync(ProductDto createProductResource, int accountId, IFormFile image);
        Task<BaseResponse<ProductDto>> UpdateAsync(int id, ProductDto request, int userId);
        Task<BaseResponse<ProductDto>> RemoveAsync(int id, int userId);
        Task<PaginationResponse<IEnumerable<ProductDto>>> GetPaginationAsync(QueryResource pagination, ProductDto filterResource);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllByCategoryIdAsync(int categoryId);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllMyProductsAsync(int userId);
    }
}
