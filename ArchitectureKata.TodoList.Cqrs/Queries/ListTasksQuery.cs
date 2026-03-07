using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Cqrs.Queries;

public class ListTasksQuery : IQuery<ListTasksRequest, IReadOnlyList<TaskItem>>
{
    private readonly ITaskRepository _taskRepository;

    public ListTasksQuery(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IReadOnlyList<TaskItem>> ExecuteAsync(ListTasksRequest request, CancellationToken cancellationToken = default)
    {
        return await _taskRepository.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}
