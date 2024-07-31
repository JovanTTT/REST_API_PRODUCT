using FluentValidation;
using System;

namespace Product.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = String.Empty;

        public int Price { get; set; } = 0;

        public ProductDTO() { }

        public ProductDTO(int id, string name, string description, int price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }

    public class PersonValidator : AbstractValidator<ProductDTO>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(0, 10);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
