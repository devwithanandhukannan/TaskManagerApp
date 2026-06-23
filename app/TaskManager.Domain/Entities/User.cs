namespace TaskManager.Domain.Entities;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; 
    public string CustomDomain { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
    public List<TaskItem> Tasks { get; set; } = [];
}


