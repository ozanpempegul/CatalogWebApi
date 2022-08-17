using CatalogWebApi.Base;
using CatalogWebApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApi.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext Context) : base(Context)
        {
        }

        public async Task<(IEnumerable<Product> records, int total)> GetPaginationAsync(QueryResource pagination, ProductDto filterResource)
        {
            var queryable = ConditionFilter(filterResource);

            var total = await queryable.CountAsync();

            var records = await queryable.AsNoTracking()
                .AsSplitQuery()
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (records, total);
        }
        private IQueryable<Product> ConditionFilter(ProductDto filterResource)
        {
            var queryable = Context.Product.AsQueryable();

            if (filterResource != null)
            {
                if (!string.IsNullOrEmpty(filterResource.Name))
                {
                    string Name = filterResource.Name.RemoveSpaceCharacter().ToLower();
                    queryable = queryable.Where(x => x.Name.Contains(Name));
                }
            }

            return queryable;
        }

        public void RemoveAsync(Product product)
        {
            Context.Remove(product);
        }

        // TO DO why is image null?
        public override async Task<Product> GetByIdAsync(int id)
        {
            var result = await Context.Product.AsSplitQuery().SingleOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId)
        {
            return Context.Product.AsSplitQuery().Where(x => x.CategoryId == categoryId).ToList();
        }

        public async Task<bool> GetIsOfferable(int id)
        {
            var tempProduct = await GetByIdAsync(id);
            return tempProduct.IsOfferable;
        }

        public async Task<IEnumerable<Product>> GetAllMyProductsAsync(int userId)
        {
            return Context.Product.AsSplitQuery().Where(x => x.AccountId == userId).ToList();
        }

        public void SellAsync(Product product)
        {
            //entities.Remove(entity);
            product.GetType().GetProperty("issold").SetValue(product, true);
        }
    }
}
