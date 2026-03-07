using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.UseCases;

public class LoginUseCase : IUseCase<LoginRequest, LoginResult>
{
    private readonly IQuery<LoginRequest, LoginResult> _query;

    public LoginUseCase(IQuery<LoginRequest, LoginResult> query)
    {
        _query = query;
    }

    public Task<LoginResult> ExecuteAsync(LoginRequest input, CancellationToken cancellationToken = default)
        => _query.ExecuteAsync(input, cancellationToken);
}
