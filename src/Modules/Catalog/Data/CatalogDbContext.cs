using Microsoft.EntityFrameworkCore;

namespace Catalog.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options):base(options) { }
        public DbSet<Product> Products  => Set<Product>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(c=>c.Name).IsRequired().HasMaxLength(100);
            base.OnModelCreating(modelBuilder);
        }
    }
}

