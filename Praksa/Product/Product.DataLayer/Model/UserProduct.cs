using Product.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.DataLayer.Model
{
    public class UserProduct
    {
        public int UserId { get; set; }
        public User User { get; set; } = new User();

        public int ProductId { get; set; }
        public ProductModel Product { get; set; } = new ProductModel();
    }
}
