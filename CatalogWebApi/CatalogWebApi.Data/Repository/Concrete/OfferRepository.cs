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
    }    
}
