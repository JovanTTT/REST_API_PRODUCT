using Moq;
using Product.BusinessLayer.Service;
using Product.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Product.BusinessLayer
{
    public class ServiceTest
    {

        private readonly IUserService _userService; // Assuming IUserService is the interface for your user service

        public ServiceTest()
        {
            // Initialize _userService, possibly using a mocking framework like Moq
            // For example, if using Moq:
            var mockUserService = new Mock<IUserService>();

            // Setup mock responses for your service here, if needed
            // mockUserService.Setup(s => s.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(...);

            _userService = mockUserService.Object;
        }

        [Theory]
        [InlineData(1, "user1", "password1", "Admin")]
        [InlineData(2, "user2", "password2", "User")]
        [InlineData(3, "user3", "password3", "Admin")]
        public async Task Can_Add_Two_Numbers_Data_Driven(int id, string username, string password, string role)
        {
        // Arrange
        var expectedResult = new User
            {
                Id = id,
                Username = username,
                PasswordHash = password,
                Role = role
            };

            // Act
            var actualResult = await _userService.GetUserByUsernameAsync(username);

            // Assert
            Assert.NotNull(actualResult); // Check if the result is not null
            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.Username, actualResult.Username);
            Assert.Equal(expectedResult.PasswordHash, actualResult.PasswordHash);
            Assert.Equal(expectedResult.Role, actualResult.Role);
        }
    }
}
