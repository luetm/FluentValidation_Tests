using FluentAssertions;
using SharedModels;

namespace Blazored.FluentValidation.Tests.Server;

[Collection("Basic Validation Tests")]
public class BasicValidation
{
    private readonly SharedTestContext _testContext;

    public BasicValidation(SharedTestContext testContext)
    {
        _testContext = testContext;
    }
    
    [Fact]
    public async Task Save_ValidDataEntered_NoValidationErrors()
    {
        // Arrange
        var fixture = new Fixture();
        var person = fixture.ValidPerson();
        var page = await _testContext.Browser.NewPageAsync(new()
        {
            BaseURL = SharedTestContext.AppUrl
        });
        await page.GotoAsync("/");
        
        // Act
        await page.FillAsync($"input[name={nameof(person.FirstName)}]", person.FirstName!);
        await page.FillAsync($"input[name={nameof(person.LastName)}]", person.LastName!);
        await page.FillAsync($"input[name={nameof(person.Age)}]", person.Age.ToString()!);
        await page.FillAsync($"input[name={nameof(person.EmailAddress)}]", person.EmailAddress!);
        await page.FillAsync($"input[name={nameof(person.Address.Line1)}]", person.Address.Line1!);
        await page.FillAsync($"input[name={nameof(person.Address.Town)}]", person.Address.Town!);
        await page.FillAsync($"input[name={nameof(person.Address.Postcode)}]", person.Address.Postcode!);
        await page.FillAsync($"input[name={nameof(person.Address.County)}]", person.Address.County!);
        await page.ClickAsync("button[type=submit]");
        
        // Assert
        (await page.Locator($"input[name={nameof(person.FirstName)}]").GetAttributeAsync("class")).Should().ContainAll("modified", "valid");
    }


    private class Fixture
    {
        public Person ValidPerson() => new()
        {
            FirstName = "John",
            LastName = "Doe",
            Age = 30,
            EmailAddress = "john.doe@gmail.com",
            Address = new()
            {
                Line1 = "123 Main St",
                Town = "New York",
                Postcode = "NY",
                County = "US",
            }
        };
    }
}