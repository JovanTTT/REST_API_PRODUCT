using Product.BusinessLayer.DTO;
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
        Task<UsetDTO> AddUser(RegisterDTO newUser);

        Task<User> Login(LoginDTO login);

        Task<User> GetUserById(int id);

        UsetDTO MapToUserDTO(User user);

        Task<int> SaveAsync();
    }
}
