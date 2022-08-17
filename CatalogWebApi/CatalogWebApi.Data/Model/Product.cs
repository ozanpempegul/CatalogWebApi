using CatalogWebApi.Base;
using Microsoft.AspNetCore.Http;
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

        public bool IsDeleted { get; set; }

        public bool IsOfferable { get; set; }

        [ForeignKey("accountid")]
        public int AccountId { get; set; }        

        public bool IsUsed { get; set; }

        public int Price { get; set; }

        [NotMapped]
        public byte[] Image { get; set; }

        public bool issold { get; set; }

    }
}
