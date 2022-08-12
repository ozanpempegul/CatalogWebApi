using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public interface IPersonService : IBaseService<PersonDto, Person>
    {
        Task<PaginationResponse<IEnumerable<PersonDto>>> GetPaginationAsync(QueryResource pagination, PersonDto filterResource);
    }
}
