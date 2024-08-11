using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.BusinessLayer.DTO
{
    public class RegisterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public RegisterDTO(string name, string password, string email, string role)
        {
            Name = name;
            Password = password;
            Email = email;
            Role = role;
        }

        public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
        {
            public RegisterDTOValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Name).NotNull();

                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Email).NotNull();

                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Password).NotNull();

                RuleFor(x => x.Role).NotEmpty();
                RuleFor(x => x.Role).NotNull();
            }
        }
    }
}
