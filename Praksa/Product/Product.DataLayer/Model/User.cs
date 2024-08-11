using Product.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.DataLayer.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();

        public User() { }
        public User(int id, string name, string email, string passHash, string role)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passHash;
            Role = role;
        }
    }
}
