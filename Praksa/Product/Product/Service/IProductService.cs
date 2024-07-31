﻿using Product.DTO;

namespace Product.Service
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();

        Task<ProductDTO> GetProductByIdAsync(int id);
    }
}