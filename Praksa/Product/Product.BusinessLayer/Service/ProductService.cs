using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.BusinessLayer.DTO;
using Product.BusinessLayer.Service;
using Product.DTO;
using Product.Model;
using Product.Repository;

namespace Product.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productsRepository;

        private readonly IUserService usersService;
        public ProductService(IProductRepository productsRepository, IUserService userService)
        {
            this.productsRepository = productsRepository;
            this.usersService = userService;
        }


        public ProductDTO MapToProductDTO(ProductModel product)
        {
            return new ProductDTO(product.Id, product.Name, product.Description, product.Price, product.OwnerId);
        }

        public ProductModel MapToProductEntity(ProductDTO productDTO)
        {
            return new ProductModel(productDTO.Id, productDTO.Name, productDTO.Description, productDTO.Price, productDTO.OwnerId);
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                List<ProductModel> allProducts = await productsRepository.GetProductsAsync();

                List<ProductDTO> dtos = [];
                foreach (ProductModel product in allProducts)
                {
                    dtos.Add(MapToProductDTO(product));
                }
                return dtos;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ProductDTO>> GetProductsForUserAsync(int userId)
        {
            try
            {
                var userWithProducts = await productsRepository.GetUserWithProductsAsync(userId);

                if (userWithProducts == null)
                {
                    throw new Exception("User not found");
                }

                // Map products to DTOs
                List<ProductDTO> dtos = userWithProducts.Products.Select(p => MapToProductDTO(p)).ToList();
                return dtos;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductDTO> AddProduct(ProductDTO newProduct, int userId)
        {
            try
            {
                newProduct.OwnerId = userId;
                var prod = MapToProductEntity(newProduct);
                var user = await usersService.GetUserById(userId);
                prod.Users.Add(user);
                user.Products.Add(prod);
                await usersService.SaveAsync();
                await productsRepository.AddProductAsync(prod);
                return newProduct;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductDTO> GetProduct(int id)
        {
            try
            {
                ProductModel product = await productsRepository.GetProductByIdAsync(id);
                return MapToProductDTO(product);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO product, int userId)
        {
            try
            {
                ProductModel prod = await productsRepository.GetProductByIdAsync(product.Id);
                if (prod.OwnerId == userId)
                {
                    prod!.Name = product.Name;
                    prod!.Description = product.Description;
                    prod!.Price = product.Price;
                    await productsRepository.SaveAsync();
                    return MapToProductDTO(prod);
                }
                throw new DbUpdateConcurrencyException();
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductDTO> DeleteProductById(int id, int userId)
        {
            try
            {
                ProductModel product = await productsRepository.GetProductByIdAsync(id);
                if (product.OwnerId == userId)
                {
                    ProductModel prod = await productsRepository.DeleteProductAsync(product, userId);
                    return MapToProductDTO(prod);
                }
                throw new ArgumentException();
            }
            catch
            {
                throw;
            }
        }

        public async Task<UsetDTO> AssignProductToUser(int productId, int userId)
        {
            try
            {
                // Fetch the user and product from the database
                var user = await usersService.GetUserById(userId);
                var product = await productsRepository.GetProductByIdAsync(productId);


                // Check if the product is already assigned to the user
                if (!user.Products.Contains(product))
                {
                    product.Users.Add(user);
                    await productsRepository.SaveAsync();
                }

                // Map the user to a UserDTO and return
                var userDTO = usersService.MapToUserDTO(user);
                return userDTO;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, object>> GetProductStatisticsAsync()
        {
            var products = await productsRepository.GetProductsAsync();

            var totalProducts = products.Count();
            var averagePrice = products.Average(p => p.Price);
            var minPrice = products.Min(p => p.Price);
            var maxPrice = products.Max(p => p.Price);
            var totalAssignments = products.Sum(p => p.Users.Count);

            return new Dictionary<string, object>
            {
                { "TotalProducts", totalProducts },
                { "AveragePrice", averagePrice },
                { "MinPrice", minPrice },
                { "MaxPrice", maxPrice },
                { "TotalAssignments", totalAssignments }
            };
        }

        public async Task<List<ProductPopularityDTO>> GetMostPopularProductsAsync(int topN)
        {
            var products = await productsRepository.GetProductsAsync();
            var popularProducts = products
                .Where(p => p.Users.Count > 0)
                .OrderByDescending(p => p.Users.Count)
                .Take(topN)
                .Select(async p =>
                {
                    var creator = await usersService.GetUserById(p.OwnerId); // Assuming OwnerId is the creator's ID
                    return new ProductPopularityDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        AssignmentCount = p.Users.Count,
                        CreatorName = creator.Name
                    };
                });

            var result = await Task.WhenAll(popularProducts);
            return result.ToList();
        }
    }
}
