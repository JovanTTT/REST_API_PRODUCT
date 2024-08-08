using Product.DataLayer.Model;

namespace Product.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; } = 0;

        // Remove OwnedId if you don't need it for this relationship
        // Add a navigation property for the many-to-many relationship
        public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();

        // Convenience property to get User IDs
        public IEnumerable<int> OwnedUserIds => UserProducts.Select(up => up.UserId);

        public ProductModel() { }

        public ProductModel(int id, string name, string description, int price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
