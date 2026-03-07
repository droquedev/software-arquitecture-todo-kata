namespace ArchitectureKata.TodoList.Cqrs;

public interface ICommand<TRequest, TResult>
{
    Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}
