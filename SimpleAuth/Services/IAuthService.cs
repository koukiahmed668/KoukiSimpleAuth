using SimpleAuth.Models;

namespace SimpleAuth.Services
{
    public interface IAuthService
    {
        // Update the RegisterAsync method to accept UserRegisterDTO
        Task<UserDTO> RegisterAsync(UserDTO userDTO);
        Task<string> LoginAsync(string username, string password);
        Task<string> GenerateJwtToken(User user);
    }
}
