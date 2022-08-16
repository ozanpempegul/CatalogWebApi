namespace CatalogWebApi.Data
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        void RemoveAsync(Offer offer);
    }
}
