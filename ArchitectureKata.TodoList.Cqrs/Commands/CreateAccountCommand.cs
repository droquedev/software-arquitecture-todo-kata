using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Cqrs.Commands;


public class CreateAccountCommand : ICommand<CreateAccountRequest, CreateAccountResult>
{
    private readonly IUserRepository _userRepository;

    public CreateAccountCommand(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CreateAccountResult> ExecuteAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return new CreateAccountResult(false, Error: "Username is required.");
        if (string.IsNullOrWhiteSpace(request.Password))
            return new CreateAccountResult(false, Error: "Password is required.");

        var existing = await _userRepository.GetByUsernameAsync(request.Username.Trim(), cancellationToken);
        if (existing != null)
            return new CreateAccountResult(false, Error: "Username already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username.Trim(),
            PasswordHash = PasswordHasher.Hash(request.Password)
        };
        await _userRepository.AddAsync(user, cancellationToken);
        return new CreateAccountResult(true, user.Id);
    }
}
