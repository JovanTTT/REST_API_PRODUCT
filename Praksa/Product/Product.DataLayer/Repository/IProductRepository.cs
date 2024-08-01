﻿using Product.Model;

namespace Product.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetAllProductsAsync();

        Task<ProductModel> GetProductByIdAsync(int id);

        Task<List<ProductModel>> AddProductAsync(ProductModel productModel);

        Task<ProductModel> UpdateProductAsync(ProductModel productModel);

        Task<bool> DeleteProductAsync(int id);
    }
}