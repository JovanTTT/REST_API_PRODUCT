using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Product.BusinessLayer.DTO;
using Product.Data;
using Product.DataLayer.Model;
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

        private readonly IUsersRepository usersRepository;

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

        public async Task<UsetDTO> AddUser(RegisterDTO newUser)
        {
                string passHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                User user = new User(0, newUser.Name, newUser.Email, passHash, newUser.Role);
                await usersRepository.AddUserAsync(user);
                return new UserDTO(user.Id, user.Name, user.Email, user.Role);
        }

    }
}
