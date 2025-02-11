using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IUserService
    {
        Task<User?> ValidateUser(string email, string password);
        Task<bool> RegisterUser(string fullName, string email, string password);
    }
}
