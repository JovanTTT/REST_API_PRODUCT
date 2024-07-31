using FluentValidation;
using Product.DTO;
using Product.Model;
using Product.Repository;

namespace Product.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IValidator<ProductDTO> _validator;

        public ProductService(IProductRepository productRepository, IValidator<ProductDTO> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
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

        public async Task<List<ProductDTO>> AddProductAsync(ProductDTO productDTO)
        {
            FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(productDTO);
            if (!result.IsValid)
            {
                throw new FluentValidation.ValidationException("Does not meet required format!");
            }

            var productModel = new ProductModel
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Description =productDTO.Description
            };

            var productModels = await _productRepository.AddProductAsync(productModel);
            return productModels.Select(pm => new ProductDTO
            {
                Id = pm.Id,
                Name = pm.Name,
                Price = pm.Price,
                Description = pm.Description
            }).ToList();
        }
    }
}
