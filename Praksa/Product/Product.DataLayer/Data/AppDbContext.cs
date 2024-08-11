using Microsoft.EntityFrameworkCore;
using Product.DataLayer.Model;
using Product.Model;

namespace Product.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Products).WithMany(u => u.Users).UsingEntity(j => j.ToTable("UserProducts"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
