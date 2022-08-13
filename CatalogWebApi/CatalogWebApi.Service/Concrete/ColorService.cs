using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public class ColorService : BaseService<ColorDto, Color>, IColorService
    {
        public ColorService(IColorRepository colorRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(colorRepository, mapper, unitOfWork)
        {
        }

    }
}
