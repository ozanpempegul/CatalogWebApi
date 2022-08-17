using CatalogWebApi.Base;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Dto
{
    public class ProductDto : BaseDto
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ColorId{ get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public bool IsOfferable { get; set; } = false;

        [Required]
        public bool IsUsed { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
