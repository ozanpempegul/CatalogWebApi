namespace CatalogWebApi.Data
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext Context) : base(Context)
        {
        }
    }
}
