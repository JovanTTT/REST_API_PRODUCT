using Product.DTO;
using Product.Model;

namespace Product.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetAllProductsAsync();

        Task<ProductDTO> GetProductByIdAsync(int id);
    }
}
