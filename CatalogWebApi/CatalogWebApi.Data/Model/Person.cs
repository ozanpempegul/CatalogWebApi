using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Data
{

    public class Person : BaseModel
    {        
        public int accountid { get; set; }        
        public string firstname { get; set; }
        public string lastname { get; set; }        
        public string email { get; set; }
        public string description{ get; set; }
        public string phone { get; set; }
        public DateTime dateofbirth { get; set; }
                
        [Key]
        public int id { get; set; }
    }
}
