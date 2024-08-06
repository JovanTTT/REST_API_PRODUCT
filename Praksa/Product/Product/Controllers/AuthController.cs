using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.BusinessLayer.DTO;
using Product.BusinessLayer.Service;
using Product.Data;
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
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context, IConfiguration configuration, IUserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
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
        public async Task<ActionResult<string>> Login(LoginDTO request)
        {
            //var user = await _userService.GetUserByUsernameAsync(request.Username);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);


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
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

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
