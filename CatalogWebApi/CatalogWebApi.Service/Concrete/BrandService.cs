using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public class BrandService : BaseService<BrandDto, Brand>, IBrandService
    {
        public BrandService(IBrandRepository brandRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(brandRepository, mapper, unitOfWork)
        {
        }

    }
}