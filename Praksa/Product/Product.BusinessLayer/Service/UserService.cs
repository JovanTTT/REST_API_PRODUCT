using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Product.BusinessLayer.DTO;
using Product.Data;
using Product.DataLayer.Model;
using Product.DataLayer.Repository;
using Product.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.BusinessLayer.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        private readonly IUserRepository _userRepository;

        private readonly IProductRepository _productRepository;


        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<string> BuyProductAsync(int userId, int productId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found.", nameof(productId));
            }

            var userProduct = new UserProduct
            {
                UserId = userId,
                ProductId = productId
            };

            try
            {
                await _userRepository.AddUserProductAsync(userProduct);
                string aa = "Ok";
                return aa;
            }
            catch (Exception ex)
            {
                // Log the exception
                //_logger.LogError(ex, "Error occurred while adding product to user.");
                string bad = "Bad request";
                return bad;
            }
        }
    }
}
