using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Cqrs;

public interface IUserRepository : IRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
