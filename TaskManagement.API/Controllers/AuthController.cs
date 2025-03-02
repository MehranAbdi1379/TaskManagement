using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services;
using TaskManagement.Shared.DTOs.Authentication;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Username,FirstName = model.FirstName, LastName = model.LastName};
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var token = await _jwtService.GenerateToken(user);
                
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents access from JavaScript
                Secure = true,   // Ensures it's only sent over HTTPS
                SameSite = SameSiteMode.None, // Prevents CSRF
                Expires = DateTime.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok("User created successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid credentials");

            var token = await _jwtService.GenerateToken(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents access from JavaScript
                Secure = true,   // Ensures it's only sent over HTTPS
                SameSite = SameSiteMode.None, // Prevents CSRF
                Expires = DateTime.UtcNow.AddMinutes(5)
            };

            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok("Login successful");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1), 
                HttpOnly = true, 
                Secure = true,    
                SameSite = SameSiteMode.None, 
            });
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("status")]
        public IActionResult GetAuthStatus()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Return a response indicating that the user is authenticated
                return Ok(new { isAuthenticated = true });
            }
            else
            {
                // Return a response indicating that the user is not authenticated
                return Ok(new { isAuthenticated = false });
            }
        }

        [HttpGet("get-user-id")]
        public IActionResult GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Standard Claim "nameid"
            if (userId == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            return Ok(new { UserId = userId });
        }


        [HttpPost("generateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid credentials");

            var token = await _jwtService.GenerateToken(user);

            return Ok(token);
        }
    }
}
