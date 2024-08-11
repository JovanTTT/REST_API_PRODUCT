using Product.DataLayer.Model;

namespace Product.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = double.MaxValue;

        public int OwnerId { get; set; } = int.MaxValue;

        public ICollection<User> Users { get; set; } = new List<User>();

        public ProductModel(int id, string name, string description, double price, int ownerId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            OwnerId = ownerId;
        }

        public ProductModel(int id, string name, string description, double price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
