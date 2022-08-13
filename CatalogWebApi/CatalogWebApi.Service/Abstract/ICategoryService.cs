using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public interface ICategoryService : IBaseService<CategoryDto, Category>
    {
    }
}
