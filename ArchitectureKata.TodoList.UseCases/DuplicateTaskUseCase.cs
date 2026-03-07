using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.UseCases;

public class DuplicateTaskUseCase : IUseCase<DuplicateTaskRequest, DuplicateTaskResult>
{
    private readonly ICommand<DuplicateTaskRequest, DuplicateTaskResult> _command;

    public DuplicateTaskUseCase(ICommand<DuplicateTaskRequest, DuplicateTaskResult> command)
    {
        _command = command;
    }

    public Task<DuplicateTaskResult> ExecuteAsync(DuplicateTaskRequest input, CancellationToken cancellationToken = default)
        => _command.ExecuteAsync(input, cancellationToken);
}
