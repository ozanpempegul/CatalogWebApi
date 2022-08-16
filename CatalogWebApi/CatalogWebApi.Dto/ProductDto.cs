using CatalogWebApi.Base;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool IsOfferable { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        [Required]
        public int Price { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
