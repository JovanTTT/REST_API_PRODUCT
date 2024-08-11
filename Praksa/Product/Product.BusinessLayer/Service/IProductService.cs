using Product.BusinessLayer.DTO;
using Product.DTO;
using Product.DataLayer.Model;
using Product.Model;

namespace Product.Service
{
    public interface IProductService
    {
        ProductDTO MapToProductDTO(ProductModel product);
        ProductModel MapToProductEntity(ProductDTO productDTO);
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> AddProduct(ProductDTO newProduct, int ownerId);
        Task<ProductDTO> GetProduct(int id);
        Task<ProductDTO> UpdateProduct(ProductDTO product, int userId);
        Task<ProductDTO> DeleteProductById(int id, int userId);
        Task<List<ProductDTO>> GetProductsForUserAsync(int userId);

        Task<UsetDTO> AssignProductToUser(int productId, int userId);

        Task<List<ProductPopularityDTO>> GetMostPopularProductsAsync(int topN);

        Task<Dictionary<string, object>> GetProductStatisticsAsync();

    }
}
