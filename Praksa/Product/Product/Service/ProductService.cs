using Product.DTO;
using Product.Model;
using Product.Repository;

namespace Product.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            // Assuming you have a method to map Product to ProductDTO
            return products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            }).ToList();
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var productModel = await _productRepository.GetProductByIdAsync(id);
            if (productModel == null)
            {
                return null;
            }

            return new ProductDTO
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Price = productModel.Price,
                Description = productModel.Description
            };
        }
    }
}
