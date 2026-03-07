using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.UseCases;

public class CreateTaskUseCase : IUseCase<CreateTaskRequest, CreateTaskResult>
{
    private readonly ICommand<CreateTaskRequest, CreateTaskResult> _command;

    public CreateTaskUseCase(ICommand<CreateTaskRequest, CreateTaskResult> command)
    {
        _command = command;
    }

    public Task<CreateTaskResult> ExecuteAsync(CreateTaskRequest input, CancellationToken cancellationToken = default)
        => _command.ExecuteAsync(input, cancellationToken);
}
