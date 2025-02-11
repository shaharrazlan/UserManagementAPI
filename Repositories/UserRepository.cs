using MongoDB.Driver;
using UserManagementAPI.Models;
using System.Threading.Tasks;

namespace UserManagementAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> AddUser(User user)
        {
            // Check if a user with the same email already exists
            var existingUser = await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return false; // User already exists
            }

            // Insert the new user into the database
            await _users.InsertOneAsync(user);
            return true; // User successfully added
        }
    }
}
