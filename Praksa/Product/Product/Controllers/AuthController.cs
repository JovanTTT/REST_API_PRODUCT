using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.BusinessLayer.DTO;
using Product.BusinessLayer.Service;
using Product.Data;
using Product.DataLayer.Model;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService usersService;
        private readonly IAuthService authService;
        private readonly IValidator<RegisterDTO> validator;

        public AuthController(IUserService usersService, IAuthService authService, IValidator<RegisterDTO> validator)
        {
            this.usersService = usersService;
            this.authService = authService;
            this.validator = validator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsetDTO>> Register(RegisterDTO registerDTO)
        {
            try
            {
                FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(registerDTO);
                if (!result.IsValid)
                {
                    return BadRequest("Data isn't valid");
                }
                return Ok(await usersService.AddUser(registerDTO));
            }
            catch
            {
                return BadRequest("Failed to register user");
            }
        }

        [HttpPut("login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var user = await usersService.Login(loginDTO);

                var token = authService.GenerateToken(user);
                return Ok(token);
            }
            catch
            {
                return BadRequest("Username or password are incorrect");
            }
        }
    }
}
