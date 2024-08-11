using FluentValidation;
using System;

namespace Product.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = double.MaxValue;

        public int OwnerId { get; set; } = int.MaxValue;

        public ProductDTO()
        {
        }
        public ProductDTO(int id, string name, string description, double price, int ownerId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            OwnerId = ownerId;
        }
    }

    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).NotNull();

            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Description).NotNull();

            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
