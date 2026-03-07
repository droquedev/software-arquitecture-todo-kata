using ArchitectureKata.TodoList.Cqrs;
using ArchitectureKata.TodoList.Cqrs.Models;
using ArchitectureKata.TodoList.UseCases;
using TaskStatus = ArchitectureKata.TodoList.Cqrs.Models.TaskStatus;

namespace ArchitectureKata.TodoList.App;

public class TodoConsoleApp
{
    private readonly IUseCase<CreateAccountRequest, CreateAccountResult> _createAccount;
    private readonly IUseCase<LoginRequest, LoginResult> _login;
    private readonly IUseCase<CreateTaskRequest, CreateTaskResult> _createTask;
    private readonly IUseCase<ListTasksRequest, IReadOnlyList<TaskItem>> _listTasks;
    private readonly IUseCase<EditTaskRequest, EditTaskResult> _editTask;
    private readonly IUseCase<DuplicateTaskRequest, DuplicateTaskResult> _duplicateTask;
    private User? _currentUser;

    public TodoConsoleApp(
        IUseCase<CreateAccountRequest, CreateAccountResult> createAccount,
        IUseCase<LoginRequest, LoginResult> login,
        IUseCase<CreateTaskRequest, CreateTaskResult> createTask,
        IUseCase<ListTasksRequest, IReadOnlyList<TaskItem>> listTasks,
        IUseCase<EditTaskRequest, EditTaskResult> editTask,
        IUseCase<DuplicateTaskRequest, DuplicateTaskResult> duplicateTask)
    {
        _createAccount = createAccount;
        _login = login;
        _createTask = createTask;
        _listTasks = listTasks;
        _editTask = editTask;
        _duplicateTask = duplicateTask;
    }

    public async Task<int> RunAsync()
    {
        while (true)
        {
            if (_currentUser == null)
            {
                var action = ShowTopMenu();
                if (action == TopAction.Exit)
                    return 0;
                if (action == TopAction.Login)
                    await DoLoginAsync();
                else if (action == TopAction.CreateAccount)
                    await DoCreateAccountAsync();
            }
            else
            {
                var action = ShowLandingMenu();
                if (action == LandingAction.LogoutAndExit)
                    return 0;
                if (action == LandingAction.Logout)
                    _currentUser = null;
                else if (action == LandingAction.CreateTask)
                    await DoCreateTaskAsync();
                else if (action == LandingAction.ListTasks)
                    await DoListTasksAsync();
                else if (action == LandingAction.EditTask)
                    await DoEditTaskAsync();
                else if (action == LandingAction.DuplicateTask)
                    await DoDuplicateTaskAsync();
            }
        }
    }

    private static TopAction ShowTopMenu()
    {
        Console.WriteLine();
        Console.WriteLine("--- Top Level Menu ---");
        Console.WriteLine("  1 (li)  Login");
        Console.WriteLine("  2 (ca)  Create Account");
        Console.WriteLine("  3 (e)   Exit");
        Console.Write("Choice: ");
        var input = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
        if (input is "3" or "e") return TopAction.Exit;
        if (input is "1" or "li") return TopAction.Login;
        if (input is "2" or "ca") return TopAction.CreateAccount;
        Console.WriteLine("Unknown option.");
        return TopAction.None;
    }

    private enum TopAction { None, Login, CreateAccount, Exit }

    private async Task DoLoginAsync()
    {
        Console.Write("Username: ");
        var username = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Password: ");
        var password = ReadPassword();
        if (string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Username is required.");
            return;
        }

        var result = await _login.ExecuteAsync(new LoginRequest(username, password));
        if (!result.Success)
        {
            Console.WriteLine("Login failed.");
            return;
        }
        _currentUser = result.User;
        Console.WriteLine($"Logged in as {_currentUser!.Username}.");
    }

    private async Task DoCreateAccountAsync()
    {
        Console.Write("Username: ");
        var username = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Password: ");
        var password = ReadPassword();
        var result = await _createAccount.ExecuteAsync(new CreateAccountRequest(username, password));
        if (!result.Success)
        {
            Console.WriteLine(result.Error ?? "Create account failed.");
            return;
        }
        Console.WriteLine("Account created. You can now log in.");
    }

    private static string ReadPassword()
    {
        var pass = "";
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                pass = pass[..^1];
            else if (!char.IsControl(key.KeyChar))
                pass += key.KeyChar;
        }
        Console.WriteLine();
        return pass;
    }

    private static LandingAction ShowLandingMenu()
    {
        Console.WriteLine();
        Console.WriteLine("--- User Menu ---");
        Console.WriteLine("  1 (lo)  Logout");
        Console.WriteLine("  2 (lx)  Logout & Exit");
        Console.WriteLine("  3 (ct)  Create Task");
        Console.WriteLine("  4 (lt)  List all tasks");
        Console.WriteLine("  5 (et)  Edit task");
        Console.WriteLine("  6 (dt)  Duplicate task");
        Console.Write("Choice: ");
        var input = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
        if (input is "2" or "lx") return LandingAction.LogoutAndExit;
        if (input is "1" or "lo") return LandingAction.Logout;
        if (input is "3" or "ct") return LandingAction.CreateTask;
        if (input is "4" or "lt") return LandingAction.ListTasks;
        if (input is "5" or "et") return LandingAction.EditTask;
        if (input is "6" or "dt") return LandingAction.DuplicateTask;
        Console.WriteLine("Unknown option.");
        return LandingAction.None;
    }

    private enum LandingAction { None, Logout, LogoutAndExit, CreateTask, ListTasks, EditTask, DuplicateTask }

    private async Task DoCreateTaskAsync()
    {
        Console.Write("Title: ");
        var title = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Comments (optional): ");
        var comments = Console.ReadLine()?.Trim() ?? "";
        var result = await _createTask.ExecuteAsync(new CreateTaskRequest(_currentUser!.Id, title, comments));
        if (!result.Success)
            Console.WriteLine(result.Error ?? "Failed to create task.");
        else
            Console.WriteLine("Task created.");
    }

    private async Task DoListTasksAsync()
    {
        var tasks = await _listTasks.ExecuteAsync(new ListTasksRequest(_currentUser!.Id));
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks.");
            return;
        }
        for (var i = 0; i < tasks.Count; i++)
            Console.WriteLine($"  {i + 1}. [{tasks[i].Status}] {tasks[i].Title} - {tasks[i].Comments}");
    }

    private async Task DoEditTaskAsync()
    {
        var tasks = (await _listTasks.ExecuteAsync(new ListTasksRequest(_currentUser!.Id))).ToList();
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks to edit.");
            return;
        }
        for (var i = 0; i < tasks.Count; i++)
            Console.WriteLine($"  {i + 1}. [{tasks[i].Status}] {tasks[i].Title}");
        Console.Write("Task number to edit: ");
        var line = Console.ReadLine()?.Trim() ?? "";
        if (!int.TryParse(line, out var num) || num < 1 || num > tasks.Count)
        {
            Console.WriteLine("Invalid number.");
            return;
        }
        var task = tasks[num - 1];
        Console.Write($"New title (Enter to keep '{task.Title}'): ");
        var titleLine = Console.ReadLine()?.Trim();
        Console.Write($"New comments (Enter to keep): ");
        var commentsLine = Console.ReadLine()?.Trim();
        Console.Write($"Status (Pending, InProgress, Done) (Enter to keep {task.Status}): ");
        var statusLine = Console.ReadLine()?.Trim();

        string? newTitle = string.IsNullOrEmpty(titleLine) ? null : titleLine;
        string? newComments = string.IsNullOrEmpty(commentsLine) ? null : commentsLine;
        TaskStatus? newStatus = null;
        if (!string.IsNullOrEmpty(statusLine) && Enum.TryParse<TaskStatus>(statusLine, true, out var s))
            newStatus = s;

        var result = await _editTask.ExecuteAsync(new EditTaskRequest(
            task.Id,
            _currentUser!.Id,
            newTitle,
            newComments,
            newStatus));
        if (!result.Success)
            Console.WriteLine(result.Error ?? "Failed to edit task.");
        else
            Console.WriteLine("Task updated.");
    }

    private async Task DoDuplicateTaskAsync()
    {
        var tasks = (await _listTasks.ExecuteAsync(new ListTasksRequest(_currentUser!.Id))).ToList();
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks to duplicate.");
            return;
        }
        Console.WriteLine("Which task to duplicate?");
        for (var i = 0; i < tasks.Count; i++)
            Console.WriteLine($"  {i + 1}. [{tasks[i].Status}] {tasks[i].Title}");
        Console.Write("Task number to duplicate: ");
        var line = Console.ReadLine()?.Trim() ?? "";
        if (!int.TryParse(line, out var num) || num < 1 || num > tasks.Count)
        {
            Console.WriteLine("Invalid number.");
            return;
        }
        var task = tasks[num - 1];
        Console.Write($"New title (Enter to keep '{task.Title}'): ");
        var titleLine = Console.ReadLine()?.Trim();

        var result = await _duplicateTask.ExecuteAsync(new DuplicateTaskRequest(task.Id, _currentUser!.Id, titleLine));
        if (!result.Success)
            Console.WriteLine(result.Error ?? "Failed to duplicate task.");
        else
            Console.WriteLine("Task duplicated.");
    }
}
