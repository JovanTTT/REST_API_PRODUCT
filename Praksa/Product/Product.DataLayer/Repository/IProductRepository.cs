using Product.DataLayer.Model;
using Product.Model;

namespace Product.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetProductsAsync();
        Task<ProductModel> AddProductAsync(ProductModel newProduct);
        Task<ProductModel> GetProductByIdAsync(int id);
        Task<int> SaveAsync();
        Task<ProductModel> DeleteProductAsync(ProductModel product, int userId);

        Task<User> GetUserWithProductsAsync(int userId);
    }
}
