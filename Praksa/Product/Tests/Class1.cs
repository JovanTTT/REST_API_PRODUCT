using Moq;
using Product.DTO;
using Product.Model;
using Product.Repository;
using Product.Service;
using Xunit;

namespace Tests
{
    public class Class1
    {
        private readonly Mock<IProductRepository> mockProductsRepository;
        private readonly ProductService productsService;

        public Class1()
        {
            mockProductsRepository = new Mock<IProductRepository>();
            productsService = new ProductService(mockProductsRepository.Object);
        }



        [Theory]
        [InlineData(1, "Product1", "Description1", 10.0)]
        [InlineData(2, "Product2", "Description2", 20.0)]
        [InlineData(3, "Product3", "Description3", 30.0)]
        public async Task GetAllProductsAsync_ShouldReturnProductDTOList(int id, string name, string description, double price)
        {
            // Arrange
            var products = new List<ProductModel>
        {
            new ProductModel(id, name, description, price)
        };
            mockProductsRepository.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await productsService.GetAllProductsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(name, result[0].Name);
            Assert.Equal(description, result[0].Description);
            Assert.Equal(price, result[0].Price);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            mockProductsRepository.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(new List<ProductModel>());

            // Act
            var result = await productsService.GetAllProductsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            mockProductsRepository.Setup(repo => repo.GetProductsAsync()).ThrowsAsync(new System.Exception("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => productsService.GetAllProductsAsync());
        }

        [Fact]
        public async Task UpdateProduct_ShouldUpdateProductAndReturnUpdatedDTO()
        {
            // Arrange
            var productDto = new ProductDTO { Id = 1, Name = "Updated Product", Description = "Updated Description", Price = 20.0 };
            var product = new ProductModel ( 1, "Old Product", "Old Description", 10.0, 1);

            mockProductsRepository.Setup(repo => repo.GetProductByIdAsync(productDto.Id)).ReturnsAsync(product);
            mockProductsRepository.Setup(repo => repo.SaveAsync()).ReturnsAsync(3);

            // Act
            var result = await productsService.UpdateProduct(productDto, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Description, result.Description);
            Assert.Equal(productDto.Price, result.Price);

            mockProductsRepository.Verify(repo => repo.GetProductByIdAsync(productDto.Id), Times.Once);
            mockProductsRepository.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
