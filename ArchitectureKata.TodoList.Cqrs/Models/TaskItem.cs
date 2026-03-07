namespace ArchitectureKata.TodoList.Cqrs.Models;

public record TaskItem
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Comments { get; init; } = string.Empty;
    public TaskStatus Status { get; init; }
}
