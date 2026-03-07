using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;
using ArchitectureKata.TodoList.Cqrs.Queries;
using ArchitectureKata.TodoList.UseCases;
using Moq;
using Xunit;

namespace ArchitectureKata.TodoList.UnitTests;

public class LoginUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WrongPassword_ReturnsFailure()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "bob",
            PasswordHash = PasswordHasher.Hash("correct")
        };
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByUsernameAsync("bob", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var query = new LoginQuery(userRepo.Object);
        var useCase = new LoginUseCase(query);
        var result = await useCase.ExecuteAsync(new LoginRequest("bob", "wrong"));

        Assert.False(result.Success);
        Assert.Null(result.User);
    }

    [Fact]
    public async Task ExecuteAsync_CorrectPassword_ReturnsSuccessAndUser()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "bob",
            PasswordHash = PasswordHasher.Hash("correct")
        };
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByUsernameAsync("bob", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var query = new LoginQuery(userRepo.Object);
        var useCase = new LoginUseCase(query);
        var result = await useCase.ExecuteAsync(new LoginRequest("bob", "correct"));

        Assert.True(result.Success);
        Assert.NotNull(result.User);
        Assert.Equal(user.Id, result.User.Id);
        Assert.Equal("bob", result.User.Username);
    }
}
