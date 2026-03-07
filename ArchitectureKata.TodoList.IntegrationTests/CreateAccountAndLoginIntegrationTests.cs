using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Commands;
using ArchitectureKata.TodoList.Cqrs.Models;
using ArchitectureKata.TodoList.Cqrs.Queries;
using ArchitectureKata.TodoList.Infra;
using ArchitectureKata.TodoList.UseCases;
using Xunit;

namespace ArchitectureKata.TodoList.IntegrationTests;

public class CreateAccountAndLoginIntegrationTests : IDisposable
{
    private readonly string _usersPath;
    private readonly string _tasksPath;
    private readonly IUserRepository _userRepo;
    private readonly CreateAccountUseCase _createAccountUseCase;
    private readonly LoginUseCase _loginUseCase;

    public CreateAccountAndLoginIntegrationTests()
    {
        var dir = AppContext.BaseDirectory;
        _usersPath = Path.Combine(dir, $"users_{Guid.NewGuid():N}.json");
        _tasksPath = Path.Combine(dir, $"tasks_{Guid.NewGuid():N}.json");

        _userRepo = new JsonFileUserRepository(_usersPath);
        var taskRepo = new JsonFileTaskRepository(_tasksPath);

        var createAccountCommand = new CreateAccountCommand(_userRepo);
        var loginQuery = new LoginQuery(_userRepo);

        _createAccountUseCase = new CreateAccountUseCase(createAccountCommand);
        _loginUseCase = new LoginUseCase(loginQuery);
    }

    public void Dispose()
    {
        if (File.Exists(_usersPath))
            File.Delete(_usersPath);
        if (File.Exists(_tasksPath))
            File.Delete(_tasksPath);
    }

    [Fact]
    public async Task CreateAccount_ThenLoginWithWrongPassword_Fails_ThenLoginWithCorrectPassword_Succeeds()
    {
        var username = "testuser";
        var password = "mypassword";

        var createResult = await _createAccountUseCase.ExecuteAsync(new CreateAccountRequest(username, password));
        Assert.True(createResult.Success, "Create account should succeed");

        var loginFailResult = await _loginUseCase.ExecuteAsync(new LoginRequest(username, "wrongpassword"));
        Assert.False(loginFailResult.Success, "Login with wrong password should fail");

        var loginSuccessResult = await _loginUseCase.ExecuteAsync(new LoginRequest(username, password));
        Assert.True(loginSuccessResult.Success, "Login with correct password should succeed");
        Assert.NotNull(loginSuccessResult.User);
        Assert.Equal(username, loginSuccessResult.User.Username);
    }
}
