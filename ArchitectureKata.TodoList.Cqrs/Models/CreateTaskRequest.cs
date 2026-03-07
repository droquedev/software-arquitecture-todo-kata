namespace ArchitectureKata.TodoList.Cqrs.Models;

public record CreateTaskRequest(Guid UserId, string Title, string Comments = "");
