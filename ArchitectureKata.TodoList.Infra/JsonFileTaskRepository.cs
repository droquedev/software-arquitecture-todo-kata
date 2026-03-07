using System.Text.Json;
using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Infra;

public class JsonFileTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public JsonFileTaskRepository(string? filePath = null)
    {
        _filePath = string.IsNullOrWhiteSpace(filePath)
            ? Path.Combine(AppContext.BaseDirectory, "tasks.json")
            : filePath;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tasks = await LoadAsync(cancellationToken);
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public async Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tasks = await LoadAsync(cancellationToken);
        return tasks.Where(t => t.UserId == userId).ToList();
    }

    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        var tasks = await LoadAsync(cancellationToken);
        tasks.Add(task);
        await SaveAsync(tasks, cancellationToken);
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        var tasks = await LoadAsync(cancellationToken);
        var index = tasks.FindIndex(t => t.Id == task.Id);
        if (index >= 0)
        {
            tasks[index] = task;
            await SaveAsync(tasks, cancellationToken);
        }
    }

    private async Task<List<TaskItem>> LoadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
            return new List<TaskItem>();

        await using var stream = File.OpenRead(_filePath);
        var list = await JsonSerializer.DeserializeAsync<List<TaskItem>>(stream, JsonOptions, cancellationToken);
        return list ?? new List<TaskItem>();
    }

    private async Task SaveAsync(List<TaskItem> tasks, CancellationToken cancellationToken)
    {
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, tasks, JsonOptions, cancellationToken);
    }
}
