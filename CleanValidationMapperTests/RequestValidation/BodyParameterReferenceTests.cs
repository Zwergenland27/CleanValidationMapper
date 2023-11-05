using CleanValidationMapperTests.TestData;
using FluentAssertions;

namespace CleanValidationMapperTests;

public class BodyParameterReferenceTests
{
	#region Optional

	[Theory]
	[InlineData(null)]
	[InlineData("Parameter")]
	public void IsOptional_Should_ReturnInParameter(string? parameter)
	{
		//Arrange
		var body = new BodyParameterReferenceTestData.OptionalBodyParameter(parameter);

		//Assert
		body.Parameter.Should().Be(parameter);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("Parameter")]
	public void Validate_Should_NotFail(string? parameter)
	{
		//Arrange
		var body = new BodyParameterReferenceTestData.OptionalBodyParameter(parameter);

		//Act
		var result = body.Validate();

		//Assert
		result.HasFailed.Should().BeFalse();
	}

	#endregion

	#region Required

	[Fact]
	public void IsRequired_Should_ReturnInParameter_When_NotNull()
	{
		//Arrange
		var parameter = "Parameter";
		var body = new BodyParameterReferenceTestData.RequiredBodyParameter(parameter);

		//Assert
		body.Parameter.Should().Be(parameter);
	}

	[Fact]
	public void IsRequired_Should_ReturnDefault_When_Null()
	{
		//Arrange
		string? parameter = null;
		var body = new BodyParameterReferenceTestData.RequiredBodyParameter(parameter);

		//Assert
		body.Parameter.Should().Be(default);
	}

	[Fact]
	public void Validate_Should_NotFail_When_InParameterNotNull()
	{
		//Arrange
		var parameter = "Parameter";
		var body = new BodyParameterReferenceTestData.RequiredBodyParameter(parameter);

		//Act
		var result = body.Validate();

		//Assert
		result.HasFailed.Should().BeFalse();
	}

	[Fact]
	public void Validate_Should_Fail_When_InParameterNull()
	{
		//Arrange
		string? parameter = null;
		var body = new BodyParameterReferenceTestData.RequiredBodyParameter(parameter);

		//Act
		var result = body.Validate();

		//Assert
		result.HasFailed.Should().BeTrue();
	}

	#endregion
}
