using System.Text.Json;
using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Infra;

public class JsonFileUserRepository : IUserRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public JsonFileUserRepository(string? filePath = null)
    {
        _filePath = string.IsNullOrWhiteSpace(filePath)
            ? Path.Combine(AppContext.BaseDirectory, "users.json")
            : filePath;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var users = await LoadAsync(cancellationToken);
        return users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var users = await LoadAsync(cancellationToken);
        return users.FirstOrDefault(u => u.Id == id);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var users = await LoadAsync(cancellationToken);
        users.Add(user);
        await SaveAsync(users, cancellationToken);
    }

    private async Task<List<User>> LoadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
            return new List<User>();

        await using var stream = File.OpenRead(_filePath);
        var list = await JsonSerializer.DeserializeAsync<List<User>>(stream, JsonOptions, cancellationToken);
        return list ?? new List<User>();
    }

    private async Task SaveAsync(List<User> users, CancellationToken cancellationToken)
    {
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, users, JsonOptions, cancellationToken);
    }
}
