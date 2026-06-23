using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs;

public record CreateTaskRequest(
    [Required][MaxLength(200)] string Title,
    [Required] string UserId
);

public record UpdateTaskStatusRequest(
    [Required] bool IsCompleted,
    [Required] string Title
);
