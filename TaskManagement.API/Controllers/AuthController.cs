using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Authentication;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.API.Controllers;

[Route("api/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    [HttpPost("users")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user = new ApplicationUser { UserName = model.Username, Email = model.Username };
        user.SetFirstName(model.FirstName);
        user.SetLastName(model.LastName);
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var token = await _jwtService.GenerateToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents access from JavaScript
            Secure = true, // Ensures it's only sent over HTTPS
            SameSite = SameSiteMode.None, // Prevents CSRF
            Expires = DateTime.UtcNow.AddHours(5)
        };

        Response.Cookies.Append("jwt", token, cookieOptions);

        return Ok("User created successfully!");
    }

    [HttpPost("sessions")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials");

        var token = await _jwtService.GenerateToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents access from JavaScript
            Secure = true, // Ensures it's only sent over HTTPS
            SameSite = SameSiteMode.None, // Prevents CSRF
            Expires = DateTime.UtcNow.AddHours(5)
        };

        Response.Cookies.Append("jwt", token, cookieOptions);

        return Ok("Login successful");
    }

    [HttpDelete("sessions")]
    public IActionResult Logout()
    {
        Response.Cookies.Append("jwt", "", new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("sessions/status")]
    public IActionResult GetAuthStatus()
    {
        return Ok(User.Identity is { IsAuthenticated: true }
            ? new { isAuthenticated = true }
            : new { isAuthenticated = false });
    }

    [HttpGet("me")]
    public IActionResult GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized("User ID not found in token.");

        return Ok(new { UserId = userId });
    }


    [HttpPost("tokens")]
    public async Task<IActionResult> GenerateToken([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials");

        var token = await _jwtService.GenerateToken(user);

        return Ok(token);
    }
}