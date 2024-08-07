using Product.DataLayer.Model;

namespace Product.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = String.Empty;

        public int Price { get; set; } = 0;

        public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();

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
