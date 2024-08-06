using Microsoft.EntityFrameworkCore;
using Moq;
using Product.BusinessLayer.Service;
using Product.Data;
using Product.DataLayer.Model;
using Product.DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Product.BusinessLayer.UserServiceTest
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly Mock<DbSet<User>> _mockUserSet;
        private readonly Mock<AppDbContext> _mockContext;

        public UserServiceTest()
        {
            _mockUserSet = new Mock<DbSet<User>>();
            _mockContext = new Mock<AppDbContext>();

            // Set up the mock context to return the mocked DbSet
            _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);

            _userService = new UserService(_mockContext.Object);
        }

        [Theory]
        [InlineData(1, "user1", "password1", "Admin")]
        [InlineData(2, "user2", "password2", "User")]
        [InlineData(3, "user3", "password3", "Admin")]
        public void RegisterUserSheck(int id, string username, string password, string role)
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
            var actualResult = _userService.GetUserByUsernameAsync(username).Result;

            // Assert
            Assert.NotNull(actualResult); 
            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.Username, actualResult.Username);
            Assert.Equal(expectedResult.PasswordHash, actualResult.PasswordHash);
            Assert.Equal(expectedResult.Role, actualResult.Role);
        }
    }
}
