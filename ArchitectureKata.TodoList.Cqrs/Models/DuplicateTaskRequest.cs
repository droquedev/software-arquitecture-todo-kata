namespace ArchitectureKata.TodoList.Cqrs.Models;

public record DuplicateTaskRequest(Guid TaskId, Guid UserId, string? Title = null);
