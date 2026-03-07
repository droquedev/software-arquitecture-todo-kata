using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;
using TaskStatus = ArchitectureKata.TodoList.Cqrs.Models.TaskStatus;

namespace ArchitectureKata.TodoList.Cqrs.Commands;

public class CreateTaskCommand : ICommand<CreateTaskRequest, CreateTaskResult>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommand(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<CreateTaskResult> ExecuteAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return new CreateTaskResult(false, Error: "Title is required.");

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title.Trim(),
            Comments = request.Comments ?? "",
            Status = TaskStatus.Pending
        };
        await _taskRepository.AddAsync(task, cancellationToken);
        return new CreateTaskResult(true, task.Id);
    }
}
