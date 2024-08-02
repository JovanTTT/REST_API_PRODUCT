using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.BusinessLayer.DTO
{
    public class UsetDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }

        public string Role { get; set; } = string.Empty;
    }
}
