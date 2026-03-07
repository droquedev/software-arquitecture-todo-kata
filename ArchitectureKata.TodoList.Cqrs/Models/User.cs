namespace ArchitectureKata.TodoList.Cqrs.Models;

public class User
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
}
