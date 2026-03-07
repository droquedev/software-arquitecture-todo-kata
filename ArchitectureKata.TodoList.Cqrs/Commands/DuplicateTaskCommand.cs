using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Cqrs.Commands;

public class DuplicateTaskCommand : ICommand<DuplicateTaskRequest, DuplicateTaskResult>
{
    private readonly ITaskRepository _taskRepository;

    public DuplicateTaskCommand(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<DuplicateTaskResult> ExecuteAsync(DuplicateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null)
            return new DuplicateTaskResult(false, "Task not found.");
        if (task.UserId != request.UserId)
            return new DuplicateTaskResult(false, "Task not found.");

        var duplicated = task;
        duplicated = duplicated with { Id = Guid.NewGuid() };
        
        if (!string.IsNullOrEmpty(request.Title))
            duplicated = duplicated with { Title = request.Title };
        else
            duplicated = duplicated with { Title = $"Copy of {duplicated.Title}" };

        await _taskRepository.AddAsync(duplicated, cancellationToken);
        return new DuplicateTaskResult(true);
    }
}
