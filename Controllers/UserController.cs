using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Get Logged-in User Info (Requires JWT)
        [HttpGet("me")]
        [Authorize] // Requires a valid JWT token
        public async Task<IActionResult> GetUserInfo()
        {
            Console.WriteLine("📥 Incoming request to /me endpoint");
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("❌ No valid token received.");
                return Unauthorized("No token received.");
            }
            
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                return Unauthorized("Invalid token: No email claim found.");
            }

            var user = await _userRepository.GetUserByEmail(emailClaim);

            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                fullName = user.FullName
            });
        }
    }
}
