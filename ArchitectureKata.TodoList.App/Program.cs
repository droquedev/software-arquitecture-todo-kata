using ArchitectureKata.TodoList.App;
using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Commands;
using ArchitectureKata.TodoList.Cqrs.Queries;
using ArchitectureKata.TodoList.Infra;
using ArchitectureKata.TodoList.UseCases;

if (args is ["--help"] or ["-h"])
{
    var helpPath = Path.Combine(AppContext.BaseDirectory, "help.txt");
    if (File.Exists(helpPath))
        Console.WriteLine(await File.ReadAllTextAsync(helpPath));
    return 0;
}

var usersPath = Environment.GetEnvironmentVariable("TODO_USERS_FILE") ?? Path.Combine(AppContext.BaseDirectory, "users.json");
var tasksPath = Environment.GetEnvironmentVariable("TODO_TASKS_FILE") ?? Path.Combine(AppContext.BaseDirectory, "tasks.json");

IUserRepository userRepo = new JsonFileUserRepository(usersPath);
ITaskRepository taskRepo = new JsonFileTaskRepository(tasksPath);

var createAccountCommand = new CreateAccountCommand(userRepo);
var loginQuery = new LoginQuery(userRepo);
var createTaskCommand = new CreateTaskCommand(taskRepo);
var listTasksQuery = new ListTasksQuery(taskRepo);
var editTaskCommand = new EditTaskCommand(taskRepo);
var duplicateTaskCommand = new DuplicateTaskCommand(taskRepo);


var createAccountUseCase = new CreateAccountUseCase(createAccountCommand);
var loginUseCase = new LoginUseCase(loginQuery);
var createTaskUseCase = new CreateTaskUseCase(createTaskCommand);
var listTasksUseCase = new ListTasksUseCase(listTasksQuery);
var editTaskUseCase = new EditTaskUseCase(editTaskCommand);
var duplicateTaskUseCase = new DuplicateTaskUseCase(duplicateTaskCommand);

var app = new TodoConsoleApp(
    createAccountUseCase,
    loginUseCase,
    createTaskUseCase,
    listTasksUseCase,
    editTaskUseCase,
    duplicateTaskUseCase);

return await app.RunAsync();
