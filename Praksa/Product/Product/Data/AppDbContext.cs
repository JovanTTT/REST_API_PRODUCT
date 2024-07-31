using Microsoft.EntityFrameworkCore;
using Product.DTO;
using Product.Model;

namespace Product.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProductDTO> Products { get; set; }
    }
}
