using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<bool> AddUser(User user);
    }
}
