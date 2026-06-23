using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class AuthCheckerController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> CheckAuthStatus() 
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);

        var customDomain = User.FindFirst("custom_domain")?.Value;

        Console.WriteLine($"User ID: {userId}");
        Console.WriteLine($"Email: {email}");
        Console.WriteLine($"Custom Domain: {customDomain}");

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }
        
        return Ok(new { 
            Success = true, 
            UserId = userId, 
            Email = email, 
            CustomDomain = customDomain 
        }); 
    }
}
