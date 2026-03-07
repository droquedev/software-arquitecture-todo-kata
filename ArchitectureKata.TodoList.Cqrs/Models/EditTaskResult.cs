namespace ArchitectureKata.TodoList.Cqrs.Models;

public record EditTaskResult(bool Success, string? Error = null);
