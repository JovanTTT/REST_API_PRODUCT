using Microsoft.EntityFrameworkCore;
using Product.Model;

namespace Product.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProductModel> Products { get; set; }
    }
}
