using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Base
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
