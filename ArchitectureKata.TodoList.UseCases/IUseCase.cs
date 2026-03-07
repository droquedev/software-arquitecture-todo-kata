namespace ArchitectureKata.TodoList.UseCases;

public interface IUseCase<TIn, TOut>
{
    Task<TOut> ExecuteAsync(TIn input, CancellationToken cancellationToken = default);
}
