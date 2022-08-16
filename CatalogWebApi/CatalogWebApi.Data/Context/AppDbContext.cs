using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CatalogWebApi.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }        
        public DbSet<Product> Product { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Offer> Offer { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<Product>(builder =>
            {
                //builder.HasNoKey();
                builder.ToTable("product");
            });
            builder.Entity<Account>(builder =>
            {
                //builder.HasNoKey();
                builder.ToTable("account");
            });
        }
    }
}
