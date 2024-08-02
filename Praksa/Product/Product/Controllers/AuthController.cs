using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Product.BusinessLayer.DTO;
using Product.BusinessLayer.Service;
using Product.DataLayer.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UsetDTO request)
        {
            // Validate role if needed
            var validRoles = new List<string> { "Admin", "User" }; // Define valid roles
            if (!validRoles.Contains(request.Role))
            {
                return BadRequest("Invalid role specified!");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role // Assign the provided role
            };

            var createdUser = await _userService.RegisterAsync(user);

            return Ok(createdUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UsetDTO request)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);

            if (user == null)
            {
                return BadRequest("User not found!");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password!");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role) // Include the role in claims
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
