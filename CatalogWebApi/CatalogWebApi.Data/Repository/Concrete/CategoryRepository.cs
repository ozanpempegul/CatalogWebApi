namespace CatalogWebApi.Data
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext Context) : base(Context)
        {
        }

        public void RemoveAsync(Category category)
        {
            Context.Remove(category);
        }
    }
}
