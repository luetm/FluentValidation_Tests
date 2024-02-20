using FluentAssertions;

namespace Blazored.FluentValidation.UnitTests.BasicValidation;

public class BasicTests : TestContext
{
    [Fact]
    public void Validate_ValidFirstAndLastname_ValidSubmit()
    {
        // Act
        var cut = RenderComponent<BasicForm>();
        cut.Find("input[name=FirstName]").Change("John");
        cut.Find("input[name=LastName]").Change("Doe");
        cut.Find("button").Click();

        // Assert
        cut.Find("#result").TextContent.Should().Be("Valid");
    }
    
    [Fact]
    public void Validate_InvalidFirstName_InvalidSubmit()
    {
        // Act
        var cut = RenderComponent<BasicForm>();
        cut.Find("input[name=FirstName]").Change(" ");
        cut.Find("input[name=LastName]").Change("Doe");
        cut.Find("button").Click();

        // Assert
        cut.Find("#result").TextContent.Should().Be("Invalid");
    }
}