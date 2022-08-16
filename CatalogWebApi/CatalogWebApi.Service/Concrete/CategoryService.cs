using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public class CategoryService : BaseService<CategoryDto, Category>, ICategoryService
    {
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(categoryRepository, mapper, unitOfWork)
        {
            this.categoryRepository = categoryRepository;
        }

        private readonly ICategoryRepository categoryRepository;

    }
}