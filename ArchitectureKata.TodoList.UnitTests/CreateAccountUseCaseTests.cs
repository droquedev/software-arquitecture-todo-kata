using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Commands;
using ArchitectureKata.TodoList.Cqrs.Models;
using ArchitectureKata.TodoList.UseCases;
using Moq;
using Xunit;

namespace ArchitectureKata.TodoList.UnitTests;

public class CreateAccountUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_AddsUser_AndHashesPassword()
    {
        var userRepo = new Mock<IUserRepository>();
        User? capturedUser = null;
        userRepo.Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        userRepo.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => capturedUser = u)
            .Returns(Task.CompletedTask);

        var command = new CreateAccountCommand(userRepo.Object);
        var useCase = new CreateAccountUseCase(command);
        var result = await useCase.ExecuteAsync(new CreateAccountRequest("alice", "secret123"));

        Assert.True(result.Success);
        Assert.NotEqual(result.UserId, Guid.Empty);
        Assert.NotNull(capturedUser);
        Assert.Equal("alice", capturedUser.Username);
        Assert.NotEqual("secret123", capturedUser.PasswordHash);
        Assert.NotEmpty(capturedUser.PasswordHash);
        userRepo.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
