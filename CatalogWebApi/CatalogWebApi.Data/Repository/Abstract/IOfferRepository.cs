namespace CatalogWebApi.Data
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        void RemoveAsync(Offer offer);
        Task<IEnumerable<Offer>> GetByBidderId(int bidderId);
        Task<IEnumerable<Offer>> GetByProductId(int productId);
        Task<Offer> GetByIdAsync(int id);
    }
}
