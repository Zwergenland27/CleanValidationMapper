using CleanValidationMapper.Errors;
using CleanValidationMapper.InputValidation;
using FluentAssertions;

namespace CleanValidationMapperTests.InputValidation;

public class RequiredReferenceTests
{
    [Fact]
    public void Create_FromParameter_Should_SetParameter_When_ParameterValid()
    {
        //Arrange
        string? parameter = "Test";
        Error missingError = Error.Validation("Parameter.Missing", "Parameter is missing");
        var propertyToValidate = new RequiredReferenceBuilder<string>("Name");

        //Act
        propertyToValidate.FromParameter(parameter, missingError);
        var result = propertyToValidate.Create();

        //Assert
        result.HasFailed.Should().BeFalse();
        result.Value.Should().Be(parameter);
    }

    [Fact]
    public void Create_FromParameter_Should_ReturnMissing_When_ParameterNull()
    {
        //Arrange
        string? parameter = null;
        Error missingError = Error.Validation("Parameter.Missing", "Parameter is missing");
        var propertyToValidate = new RequiredReferenceBuilder<string>("Name");

        //Act
        propertyToValidate.FromParameter(parameter, missingError);
        var result = propertyToValidate.Create();

        //Assert
        result.HasFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(missingError);
    }
}
