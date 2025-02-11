using UserManagementAPI.Models;
using UserManagementAPI.Repositories;
using BCrypt.Net;
using System.Threading.Tasks;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUser(string fullName, string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmail(email);
            if (existingUser != null)
                return false; // Email already exists

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddUser(newUser);
            return true;
        }

        public async Task<User?> ValidateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; // Invalid credentials
            }
            return user; // User authenticated
        }

    }
}
