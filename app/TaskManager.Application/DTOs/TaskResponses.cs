namespace TaskManager.Application.DTOs;

public record TaskResponse(
    string Id,
    string Title,
    bool IsCompleted,
    string OwnerId
);
