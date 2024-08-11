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
        private readonly IUserRepository usersRepository;

        public UserService(IUserRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public UsetDTO MapToUserDTO(User user)
        {
            return new UsetDTO(user.Id, user.Name, user.Email, user.Role);
        }

        public async Task<UsetDTO> AddUser(RegisterDTO newUser)
        {
            try
            {
                string passHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                User user = new User(0, newUser.Name, newUser.Email, passHash, newUser.Role);
                await usersRepository.AddUserAsync(user);
                return new UsetDTO(user.Id, user.Name, user.Email, user.Role);
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> Login(LoginDTO login)
        {
            try
            {
                var user = await usersRepository.GetUserByNameAsync(login.Name);
                if (user == null) throw new Exception();

                if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash)) throw new Exception();
                return user;
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> GetUserById(int id)
        {
            try
            {
                var user = await usersRepository.GetUserByIdAsync(id);
                return user;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await usersRepository.SaveAsync();
        }
    }
}
