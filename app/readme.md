# Database
##  Install MongoDB EF Core 
```
cd TaskManager.Infrastructure
dotnet add package MongoDB.EntityFrameworkCore
```

# Solution File
```
dotnet new sln -n TaskManager

dotnet sln add TaskManager.Domain/TaskManager.Domain.csproj
dotnet sln add TaskManager.Application/TaskManager.Application.csproj
dotnet sln add TaskManager.Infrastructure/TaskManager.Infrastructure.csproj
dotnet sln add TaskManager.API/TaskManager.API.csproj

dotnet sln list
```

# json cookie (refresh and access Token)
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt

```

# activate the JWT authentication middleware in your solution
```
dotnet add TaskManager.API/TaskManager.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer

```
##  Configure and Turn On the Middleware in Program.cs
```
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructureServices(builder.Configuration);

// 🔒 1. CONFIGURE JWT BEARER AUTHENTICATION SERVICE
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 🔒 2. ACTIVATE THE AUTHENTICATION MIDDLEWARE PIPELINE
// Note: UseAuthentication must ALWAYS come before UseAuthorization!
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();

```

## Step 3: Secure Your Controller Endpoints Using [Authorize]
```
using Microsoft.AspNetCore.Authorization; // 1. ADD THIS IMPORT
using Microsoft.AspNetCore.Mvc;
// ... your other imports

namespace TaskManager.API.Controllers;

[Authorize] // 🚀 2. ADD THIS: This locks down EVERY endpoint inside this controller!
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    // Your existing GET, POST, PUT, DELETE methods remain exactly the same
}

```

## Extract the User ID Safely inside Protected Endpoints

```
using System.Security.Claims; // Required for FindFirstValue

[HttpPost]
public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request)
{
    // 🚀 Automatically extract the authenticated User ID from the JWT bearer token
    var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (string.IsNullOrEmpty(userIdFromToken))
    {
        return Unauthorized();
    }

    var newTask = new TaskItem
    {
        Id = Guid.NewGuid().ToString(),
        Title = request.Title,
        UserId = userIdFromToken, // Safe, un-tamperable ID from token
        IsCompleted = false
    };

    _context.Tasks.Add(newTask);
    await _context.SaveChangesAsync();

    // Return the response DTO mapping...
}

```