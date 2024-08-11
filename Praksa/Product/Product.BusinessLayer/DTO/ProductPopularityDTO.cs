using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.BusinessLayer.DTO
{
    public class ProductPopularityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AssignmentCount { get; set; }
        public string CreatorName { get; set; } = string.Empty;
    }
}
