using FluentAssertions;
using Microsoft.Playwright;
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
        await FillPerson(page, person);
        await page.ClickAsync("button[type=submit]");
        
        // Assert
        (await page.Locator($"input[name={nameof(person.FirstName)}]").GetAttributeAsync("class")).Should().ContainAll("modified", "valid");
    }

    [Fact]
    public async Task Enter_InvalidFirstNameEntered_ValidationErrorForFirstName()
    {
        // Arrange
        var fixture = new Fixture();
        var person = fixture.ValidPerson();
        person.FirstName = " ";
        var page = await _testContext.Browser.NewPageAsync(new()
        {
            BaseURL = SharedTestContext.AppUrl
        });
        await page.GotoAsync("/");
        
        // Act
        await FillPerson(page, person);
        await page.ClickAsync("button[type=submit]");
        
        // Assert
        (await page.Locator("ul[class=validation-errors]").InnerTextAsync()).Should().Contain("You must enter your first name");
        (await page.Locator("div[class=validation-message]").InnerTextAsync()).Should().Contain("You must enter your first name");
    }
    
    private async Task FillPerson(IPage page, Person person)
    {
        await page.FillAsync($"input[name={nameof(person.FirstName)}]", person.FirstName!);
        await page.FillAsync($"input[name={nameof(person.LastName)}]", person.LastName!);
        await page.FillAsync($"input[name={nameof(person.Age)}]", person.Age.ToString()!);
        await page.FillAsync($"input[name={nameof(person.EmailAddress)}]", person.EmailAddress!);
        await page.FillAsync($"input[name={nameof(person.Address.Line1)}]", person.Address.Line1!);
        await page.FillAsync($"input[name={nameof(person.Address.Town)}]", person.Address.Town!);
        await page.FillAsync($"input[name={nameof(person.Address.Postcode)}]", person.Address.Postcode!);
        await page.FillAsync($"input[name={nameof(person.Address.County)}]", person.Address.County!);
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