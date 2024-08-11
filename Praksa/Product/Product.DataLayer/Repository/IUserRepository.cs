using Product.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.DataLayer.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        Task<User> AddUserAsync(User newUser);

        Task<User> GetUserByNameAsync(string name);

        Task<int> SaveAsync();
    }
}
