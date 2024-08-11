using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.DataLayer.Model;
using Product.Model;

namespace Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            try
            {
                return await appDbContext.Products.Include(p => p.Users).ToListAsync();
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<ProductModel> AddProductAsync(ProductModel newProduct)
        {
            try
            {
                var product = appDbContext.Products.Add(newProduct);
                await appDbContext.SaveChangesAsync();
                return product.Entity;
            }
            catch
            {
                throw new DbUpdateException();
            }
        }
        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            try
            {
                return await appDbContext.Products.FirstOrDefaultAsync(e => e.Id == id) ?? throw new Exception(message: "Product not found");
            }
            catch
            {
                throw new Exception(message: "Product not found");
            }
        }

        public async Task<User> GetUserWithProductsAsync(int userId)
        {
            return await appDbContext.Users
                .Include(u => u.Products) // Eager loading of products
                .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new DbUpdateException();
        }

        public async Task<int> SaveAsync()
        {
            return await appDbContext.SaveChangesAsync();
        }

        public async Task<ProductModel> DeleteProductAsync(ProductModel product, int userId)
        {
            try
            {
                appDbContext.Products.Remove(product);
                await appDbContext.SaveChangesAsync();
                return product;
            }
            catch
            {
                throw new DbUpdateConcurrencyException();
            }
        }
    }
}
