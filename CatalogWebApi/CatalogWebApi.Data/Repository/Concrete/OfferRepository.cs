using Microsoft.EntityFrameworkCore;

namespace CatalogWebApi.Data
{
    public class OfferRepository : GenericRepository<Offer>, IOfferRepository
    {
        public OfferRepository(AppDbContext Context) : base(Context)
        {
        }

        public new void RemoveAsync(Offer offer)
        {
            Context.Remove(offer);
        }

        public async Task<IEnumerable<Offer>> GetByBidderId(int bidderId)
        {
            var queryable = Context.Offer.Where(x => x.BidderId.Equals(bidderId));
            return await queryable.AsNoTracking().ToListAsync();
        }

        public async Task<Offer> GetByProductId(int productId)
        {
            var queryable = Context.Offer.Where(x => x.ProductId.Equals(productId));
            return await queryable.FirstOrDefaultAsync();
        }

        public override async Task<Offer> GetByIdAsync(int id)
        {
            var result = await Context.Offer.AsSplitQuery().SingleOrDefaultAsync(x => x.Id == id);
            return result;
        }
    }    
}
