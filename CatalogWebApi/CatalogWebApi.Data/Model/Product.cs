using CatalogWebApi.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogWebApi.Data
{

    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description{ get; set; }

        [ForeignKey("categoryid")]
        public int CategoryId { get; set; }

        [ForeignKey("colorid")]
        public int ColorId { get; set; }

        [ForeignKey("brandid")]
        public int BrandId { get; set; }

        public int Id { get; set; }
    }
}
