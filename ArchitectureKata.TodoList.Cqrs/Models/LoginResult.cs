namespace ArchitectureKata.TodoList.Cqrs.Models;

public record LoginResult(bool Success, User? User = null);
