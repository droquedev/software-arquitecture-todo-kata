using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.UseCases;

public class ListTasksUseCase : IUseCase<ListTasksRequest, IReadOnlyList<TaskItem>>
{
    private readonly IQuery<ListTasksRequest, IReadOnlyList<TaskItem>> _query;

    public ListTasksUseCase(IQuery<ListTasksRequest, IReadOnlyList<TaskItem>> query)
    {
        _query = query;
    }

    public Task<IReadOnlyList<TaskItem>> ExecuteAsync(ListTasksRequest input, CancellationToken cancellationToken = default)
        => _query.ExecuteAsync(input, cancellationToken);
}
