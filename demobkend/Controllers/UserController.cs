using demobkend.Data;
using demobkend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Mail;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace demobkend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        private readonly List<string> AllowedRoles = new List<string> { "Learner", "Instructor", "Admin", "Support" };

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        { 
            var users = await _context.Users.ToListAsync();
             
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }

            return Ok(users);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validate role in a case-insensitive manner
            if (!AllowedRoles.Contains(request.Role))
                return BadRequest($"Invalid role. Allowed roles are {string.Join(", ", AllowedRoles)}.");

            // Validate email format
            var emailValidationResult = new EmailAddressAttribute().IsValid(request.Email);
            if (!emailValidationResult)
                return BadRequest("Invalid email format.");

            // Check if email or username is already registered
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email is already registered.");

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Username is already taken.");

            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create and save user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = request.Role,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Find user by username or email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid username/email or password.");

            // Validate role in a case-insensitive manner
            if (!AllowedRoles.Contains(user.Role, StringComparer.OrdinalIgnoreCase))
                return Unauthorized("Role is not valid.");

            // Generate JWT token
            var token = GenerateJwtToken(user);
            return Ok(new
            {
                Token = token,
                User = new
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    user.Role
                }
            });
        }

        [HttpPut("approve/{userId}")]
        public async Task<IActionResult> ApproveOrDeactivateUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            // Toggle the user's active state
            user.IsActive = !user.IsActive;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Return a proper JSON response
            return Ok(new
            {
                message = $"User {(user.IsActive ? "activated" : "deactivated")} successfully.",
                userId = user.UserId,
                isActive = user.IsActive
            });
        }


        private string GenerateJwtToken(User user)
        {
            Console.WriteLine($"Generating token for user: {user.Username}, Role: {user.Role}");  // Debugging line

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            var claims = new[]
            {
        new System.Security.Claims.Claim("sub", user.UserId.ToString()),
        new System.Security.Claims.Claim("name", user.Username),
        new System.Security.Claims.Claim("email", user.Email),
        new System.Security.Claims.Claim("role", user.Role),
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPut("{userId}")] 
        public async Task<IActionResult> EditUser(int userId, [FromBody] EditUserRequest request)
        { 
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }
             
            user.Username = request.Username ?? user.Username;
            user.Email = request.Email ?? user.Email;
            user.Role = request.Role ?? user.Role;
             
            if (!string.IsNullOrEmpty(request.Role) && !AllowedRoles.Contains(request.Role))
            {
                return BadRequest($"Invalid role. Allowed roles are {string.Join(", ", AllowedRoles)}.");
            }
             
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }
             
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok($"User with ID {userId} has been updated successfully.");
        }


        [HttpDelete("{userId}")] 
        public async Task<IActionResult> DeleteUser(int userId)
        {
            // Find the user by ID
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Remove the user from the database
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok($"User with ID {userId} has been deleted successfully.");
        }


    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        [EmailAddress] // Email validation attribute
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; } // Can be email or username
        public string Password { get; set; }
    }

    public class EditUserRequest
    {
        public string Username { get; set; }
        [EmailAddress]  
        public string Email { get; set; }
        public string Password { get; set; }  
        public string Role { get; set; } 
    }

}
