using Microsoft.IdentityModel.Tokens;
using Product.BusinessLayer.Helpers;
using Product.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Product.BusinessLayer.Service
{
    public class AuthService : IAuthService
    {
        public string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(AuthSettings.PrivateKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

            var claims = GenerateClaims(user);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private static List<Claim> GenerateClaims(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            return claims;
        }
    }
}
