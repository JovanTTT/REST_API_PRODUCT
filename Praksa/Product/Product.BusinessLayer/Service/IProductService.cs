using Product.DTO;

namespace Product.Service
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();

        Task<ProductDTO> GetProductByIdAsync(int id);

        Task<List<ProductDTO>> AddProductAsync(ProductDTO productDTO);

        Task<ProductDTO> UpdateProductAsync(ProductDTO productDTO);

        Task<bool> DeleteProductAsync(int id);
    }
}
