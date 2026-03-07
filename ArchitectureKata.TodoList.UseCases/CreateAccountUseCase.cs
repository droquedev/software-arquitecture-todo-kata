using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.UseCases;

public class CreateAccountUseCase : IUseCase<CreateAccountRequest, CreateAccountResult>
{
    private readonly ICommand<CreateAccountRequest, CreateAccountResult> _command;

    public CreateAccountUseCase(ICommand<CreateAccountRequest, CreateAccountResult> command)
    {
        _command = command;
    }

    public Task<CreateAccountResult> ExecuteAsync(CreateAccountRequest input, CancellationToken cancellationToken = default)
        => _command.ExecuteAsync(input, cancellationToken);
}
