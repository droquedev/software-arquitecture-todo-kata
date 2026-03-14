# Software Architecture Todo Kata!

A console TODO list application for practicing layered architecture: App → UseCases → Commands & Queries → Repositories.

## Forking

1. Open this repository on GitHub.
2. Click **Fork** (top right).
3. Choose your account or organization as the fork destination.

## Cloning

After forking (or if you have access to the repo):

```bash
git clone https://github.com/YOUR_USERNAME/software-arquitecture-todo-kata.git
cd software-arquitecture-todo-kata
```

Replace `YOUR_USERNAME` with your GitHub username or the repo owner.

## Tooling requirements

- **git** – version control
- **.NET 10 SDK** – [Download](https://dotnet.microsoft.com/download) (or use .NET 8+ if 10 is not available)
- **VS Code** or **Visual Studio Community** – editor/IDE

Check your .NET version:

```bash
dotnet --version
```

## Building

From the repository root:

```bash
dotnet build
```

To build a specific project:

```bash
dotnet build ArchitectureKata.TodoList.App/ArchitectureKata.TodoList.App.csproj
```

## Running tests

```bash
dotnet test
```

- **Unit tests** – `ArchitectureKata.TodoList.UnitTests` (mocked repositories)
- **Integration tests** – `ArchitectureKata.TodoList.IntegrationTests` (real JSON files, cleaned up after run)

## Running the app

```bash
dotnet run --project ArchitectureKata.TodoList.App
```

Show help (reads and prints `help.txt`):

```bash
dotnet run --project ArchitectureKata.TodoList.App -- --help
```

Or using the built executable:

```bash
dotnet ArchitectureKata.TodoList.App/bin/Debug/net10.0/ArchitectureKata.TodoList.App.dll --help
```

### Menus

- **Top level:** Login (1/li), Create Account (2/ca), Exit (3/e)
- **After login:** Logout (1/lo), Logout & Exit (2/lx), Create Task (3/ct), List tasks (4/lt), Edit task (5/et)

Data is stored in JSON files in the application directory by default (`users.json`, `tasks.json`). You can override paths with environment variables `TODO_USERS_FILE` and `TODO_TASKS_FILE`.
