using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Cqrs.Commands;

public class EditTaskCommand : ICommand<EditTaskRequest, EditTaskResult>
{
    private readonly ITaskRepository _taskRepository;

    public EditTaskCommand(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<EditTaskResult> ExecuteAsync(EditTaskRequest request, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null)
            return new EditTaskResult(false, "Task not found.");
        if (task.UserId != request.UserId)
            return new EditTaskResult(false, "Task not found.");

        var updated = task;
        if (request.Title != null)
            updated = updated with { Title = request.Title };
        if (request.Comments != null)
            updated = updated with { Comments = request.Comments };
        if (request.Status.HasValue)
            updated = updated with { Status = request.Status.Value };

        await _taskRepository.UpdateAsync(updated, cancellationToken);
        return new EditTaskResult(true);
    }
}
