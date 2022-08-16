namespace CatalogWebApi.Data
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void RemoveAsync(Category category);
    }
}
