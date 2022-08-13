using CatalogWebApi.Base;

namespace CatalogWebApi.Data
{
    public class Account : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Nullable<DateTime> LastActivity { get; set; }
        public bool isdeleted { get; set; }

    }
}
