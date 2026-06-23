using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs;
public record RegisterUserRequest(
    [Required] string Name,
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password,
    [Required] string CustomDomain
);

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password
);

public record LoginResponse(
    string Id,
    string Name,
    string Email,
    string CustomDomain,
    string AccessToken 
);