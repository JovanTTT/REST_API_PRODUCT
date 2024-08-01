using Product.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.BusinessLayer.Service
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
