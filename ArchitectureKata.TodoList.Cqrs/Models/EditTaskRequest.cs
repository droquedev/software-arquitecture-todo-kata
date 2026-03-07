namespace ArchitectureKata.TodoList.Cqrs.Models;

public record EditTaskRequest(Guid TaskId, Guid UserId, string? Title = null, string? Comments = null, TaskStatus? Status = null);
