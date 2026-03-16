using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Build.Utilities;
using ASPServerAPI.Data;
using ASPServerAPI.DTOs;
using ASPServerAPI.Models;

namespace ASPServerAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDto?> Register(RegisterDto register)
        {
            if (await _context.Users.AnyAnync(u => u.Email == register.Email))
                return null;

            var user = new User
            {
                UserName = register.UserName,
                Email = register.Email,
                PasswordHash = register.BCrypt.Net.BCrypt.HashPassword(register.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangeAnync();

            return new AuthResponseDto
            {
                Token = GenerateToken(user),
                UserName = user.UserName
            };
        }

        public async Task<AuthResponseDto?> Login(LoginDto login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                return null;
            return new AuthResponseDto
            {
                Token = GenerateToken(user),
                UserName = user.UserName,
            };
        }

        private string GenerateToken (User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var Token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}