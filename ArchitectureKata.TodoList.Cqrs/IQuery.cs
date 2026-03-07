namespace ArchitectureKata.TodoList.Cqrs;

public interface IQuery<TRequest, TResult>
{
    Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}
