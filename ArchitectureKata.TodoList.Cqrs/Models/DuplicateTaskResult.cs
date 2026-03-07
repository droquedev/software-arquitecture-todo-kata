namespace ArchitectureKata.TodoList.Cqrs.Models;

public record DuplicateTaskResult(bool Success, string? Error = null);
