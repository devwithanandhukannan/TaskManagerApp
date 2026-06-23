public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // never used in DTO
    public string Role { get; set; } = "User";
    public List<TaskItem> Tasks { get; set; } = new();
}

