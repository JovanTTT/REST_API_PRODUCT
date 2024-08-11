using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.DataLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await appDbContext.Users.ToListAsync();
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await appDbContext.Users.FirstOrDefaultAsync(e => e.Id == id) ?? new User();
            }
            catch
            {
                throw new Exception(message: "User not found");
            }
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            try
            {
                return await appDbContext.Users.FirstOrDefaultAsync(e => e.Name == name) ?? throw new DbUpdateException();
            }
            catch
            {
                throw new DbUpdateException();
            }
        }

        public async Task<User> AddUserAsync(User newUser)
        {
            try
            {
                var user = appDbContext.Users.Add(newUser);
                await appDbContext.SaveChangesAsync();
                return user.Entity;
            }
            catch
            {
                throw new DbUpdateException();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await appDbContext.SaveChangesAsync();
        }
    }
}
