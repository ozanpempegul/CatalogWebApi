using CatalogWebApi.Base;

namespace CatalogWebApi.Data
{
    public class Offer: BaseModel
    {
        public int ProductId { get; set; }
        public int BidderId { get; set; }
        public int OfferedPrice { get; set; }

    }
}
