using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleAuth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleAuth.Services
{
    public class AuthService<TContext> : IAuthService where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly string _jwtSecret;

        public AuthService(TContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtSecret = configuration["Jwt:Key"];
        }

        public async Task<UserDTO> RegisterAsync(UserDTO userDTO)
        {
            var user = new User
            {
                Username = userDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName
            };

            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO { Username = user.Username };
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _context.Set<User>().SingleOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            return await GenerateJwtToken(user);
        }

        public Task<string> GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
