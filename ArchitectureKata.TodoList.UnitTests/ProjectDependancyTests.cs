using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using Xunit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.xUnit;

namespace ArchitectureKata.TodoList.UnitTests;

public class ProjectDependancyTests
{

    private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
      System.Reflection.Assembly.Load("ArchitectureKata.TodoList.UseCases"),
      System.Reflection.Assembly.Load("ArchitectureKata.TodoList.Infra")
    ).Build();


    private readonly IObjectProvider<IType> UseCases = Types().That().ResideInNamespace(
      "ArchitectureKata.TodoList.UseCases"
    ).As("Use cases");

    private readonly IObjectProvider<IType> ForbiddenTypes = Types().That().ResideInNamespace(
      "ArchitectureKata.TodoList.Infra"
    ).As("Forbidden");


    [Fact]
    public void UseCasesMustNotDependOnInfra()
    {
        IArchRule ShouldNotDependOn = Types().That().Are(UseCases).Should().NotDependOnAny(ForbiddenTypes)
        .Because("Use cases should not depend on infra");

        ShouldNotDependOn.Check(Architecture);
    }

}