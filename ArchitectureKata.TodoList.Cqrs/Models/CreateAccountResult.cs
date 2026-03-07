namespace ArchitectureKata.TodoList.Cqrs.Models;

public record CreateAccountResult(bool Success, Guid? UserId = null, string? Error = null);
