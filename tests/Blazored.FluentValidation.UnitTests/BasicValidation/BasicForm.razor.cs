using Blazored.FluentValidation.UnitTests.Model;

namespace Blazored.FluentValidation.UnitTests.BasicValidation;

public partial class BasicForm
{
    private readonly Person _person = new();
    private FluentValidationValidator? _fluentValidationValidator;
    private string _result = "";

    private void SubmitValidForm() =>
        _result = "Valid";

    private void SubmitInvalidForm() =>
        _result = "Invalid";
}