using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.UseCases;

public class EditTaskUseCase : IUseCase<EditTaskRequest, EditTaskResult>
{
    private readonly ICommand<EditTaskRequest, EditTaskResult> _command;

    public EditTaskUseCase(ICommand<EditTaskRequest, EditTaskResult> command)
    {
        _command = command;
    }

    public Task<EditTaskResult> ExecuteAsync(EditTaskRequest input, CancellationToken cancellationToken = default)
        => _command.ExecuteAsync(input, cancellationToken);
}
