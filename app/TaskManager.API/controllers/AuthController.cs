using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Authentication;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthController(IApplicationDbContext context, JwtTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("Email already registered.");

        var user = new User
        {
            
            Name = request.Name,
            Email = request.Email,
            CustomDomain = request.CustomDomain,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password) 
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        Console.WriteLine(user.CustomDomain);
        Console.WriteLine("reached");
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        // Generate Tokens
        var accessToken = _tokenGenerator.GenerateAccessToken(user);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        // Save refresh token to MongoDB
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,         
            Secure = true,           
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("X-Refresh-Token", refreshToken, cookieOptions);

        return Ok(new LoginResponse(user.Id, user.Name, user.Email, user.CustomDomain, accessToken));
    }
}
