using CatalogWebApi.Base;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Dto
{
    public class OfferDto: BaseDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int BidderId { get; set; }
        [Required]
        public int OfferedPrice { get; set; }
    }
}
