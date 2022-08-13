namespace CatalogWebApi.Data
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(AppDbContext Context) : base(Context)
        {
        }
    }
}
