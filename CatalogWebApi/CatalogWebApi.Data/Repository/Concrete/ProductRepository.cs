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
                //if (!string.IsNullOrEmpty(filterResource.Id.ToString()))
                //    queryable = queryable.Where(x => x.Id.Equals(filterResource.Id.ToString().RemoveSpaceCharacter()));

                if (!string.IsNullOrEmpty(filterResource.Name))
                {
                    string Name = filterResource.Name.RemoveSpaceCharacter().ToLower();
                    queryable = queryable.Where(x => x.Name.Contains(Name));
                }
            }

            return queryable;
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await Context.Product.AsSplitQuery().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> TotalRecordAsync()
        {
            return await Context.Product.CountAsync();
        }
    }
}
