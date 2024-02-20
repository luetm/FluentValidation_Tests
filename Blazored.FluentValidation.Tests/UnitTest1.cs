using Blazored.Testbed;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blazored.FluentValidation.Tests;

public class UnitTest1
{
    private readonly WebApplicationFactory<ITestReference> _factory = new();
    
    [Fact]
    public void Test1() { }
}