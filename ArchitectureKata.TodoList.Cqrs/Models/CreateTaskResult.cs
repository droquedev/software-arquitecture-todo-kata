namespace ArchitectureKata.TodoList.Cqrs.Models;

public record CreateTaskResult(bool Success, Guid? TaskId = null, string? Error = null);
