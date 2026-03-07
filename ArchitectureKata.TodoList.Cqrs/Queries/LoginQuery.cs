using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;

namespace ArchitectureKata.TodoList.Cqrs.Queries;

public class LoginQuery : IQuery<LoginRequest, LoginResult>
{
    private readonly IUserRepository _userRepository;

    public LoginQuery(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<LoginResult> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null)
            return new LoginResult(false);

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            return new LoginResult(false);

        return new LoginResult(true, user);
    }
}
