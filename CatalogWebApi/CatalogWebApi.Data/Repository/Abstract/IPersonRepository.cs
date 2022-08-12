namespace CatalogWebApi.Data
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<(IEnumerable<Person> records, int total)> GetPaginationAsync(QueryResource pagination, PersonDto filterResource);
        Task<int> TotalRecordAsync();
    }
}
