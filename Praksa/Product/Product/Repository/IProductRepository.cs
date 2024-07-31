﻿using Product.Model;

namespace Product.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetAllProductsAsync();

        Task<ProductModel> GetProductByIdAsync(int id);
    }
}
