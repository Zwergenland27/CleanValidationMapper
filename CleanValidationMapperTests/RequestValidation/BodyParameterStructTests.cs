using CleanValidationMapperTests.TestData;
using FluentAssertions;

namespace CleanValidationMapperTests;

public class BodyParameterStructTests
{
	#region Optional

	[Theory]
	[InlineData(null)]
	[InlineData(42)]
	public void IsOptional_Should_ReturnInParameter(int? parameter)
	{
		//Arrange
		var body = new BodyParameterStructTestData.OptionalBodyParameter(parameter);

		//Assert
		body.Parameter.Should().Be(parameter);
	}

	[Theory]
	[InlineData(null)]
	[InlineData(42)]
	public void Validate_Should_NotFail(int? parameter)
	{
		//Arrange
		var body = new BodyParameterStructTestData.OptionalBodyParameter(parameter);

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
		var parameter = 42;
		var body = new BodyParameterStructTestData.RequiredBodyParameter(parameter);

		//Assert
		body.Parameter.Should().Be(parameter);
	}

	[Fact]
	public void IsRequired_Should_ReturnDefault_When_Null()
	{
		//Arrange
		int? parameter = null;
		var body = new BodyParameterStructTestData.RequiredBodyParameter(parameter);

		//Assert
		body.Parameter.Should().Be(default);
	}

	[Fact]
	public void Validate_Should_NotFail_When_InParameterNotNull()
	{
		//Arrange
		var parameter = 42;
		var body = new BodyParameterStructTestData.RequiredBodyParameter(parameter);

		//Act
		var result = body.Validate();

		//Assert
		result.HasFailed.Should().BeFalse();
	}

	[Fact]
	public void Validate_Should_Fail_When_InParameterNull()
	{
		//Arrange
		int? parameter = null;
		var body = new BodyParameterStructTestData.RequiredBodyParameter(parameter);

		//Act
		var result = body.Validate();

		//Assert
		result.HasFailed.Should().BeTrue();
	}

	#endregion
}
