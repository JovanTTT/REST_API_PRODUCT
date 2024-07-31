using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.DTO;
using Product.Model;

namespace Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return product != null ? new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            } : null;
        }

        public async Task<List<ProductModel>> AddProductAsync(ProductModel productModel)
        {
            _context.Products.Add(productModel);
            await _context.SaveChangesAsync();
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel productModel)
        {
            _context.Products.Update(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }
    }
}
