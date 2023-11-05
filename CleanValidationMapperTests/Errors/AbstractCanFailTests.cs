using CleanValidationMapper.Errors;
using CleanValidationMapper;
using Moq;
using CleanValidationMapperTests.TestData;
using FluentAssertions;

namespace CleanValidationMapperTests.Errors;

public class AbstractCanFailTests
{
	private static void ValidateErrorsSetCorrectly(Error inList, Error original)
	{
		inList.Should().Be(original);
	}

	[Theory]
	[ClassData(typeof(ErrorTestData.ErrorLists))]
	public void Failed_Should_AddErrors(List<Error> errors)
	{
		//Arrange
		CanFail result = new();

		//Act
		errors.ForEach(error =>
		{
			result.Failed(error);
		});

		//Assert
		result.HasFailed.Should().BeTrue();
		result.Errors.Zip(errors).ToList().ForEach(pair => ValidateErrorsSetCorrectly(pair.First, pair.Second));
	}

	[Theory]
	[ClassData(typeof(CanFailTestData.ResultLists))]
	public void Failed_Should_AddErrorsFromResults(List<AbstractCanFail> results)
	{
		//Arrange
		CanFail finalResult = new();

		//Act
		results.ForEach(result =>
		{
			finalResult.Failed(result);
		});

		//Assert
		finalResult.HasFailed.Should().BeTrue();

		results.ForEach(result =>
		{
			finalResult.Errors.Zip(result.Errors).ToList().ForEach(pair => ValidateErrorsSetCorrectly(pair.First, pair.Second));
		});
	}

	[Fact]
	public void Type_Should_ReturnNone_When_NoErrors()
	{
		//Arrange
		AbstractCanFail result = new();

		//Act
		var resultType = result.Type;

		//Assert
		resultType.Should().Be(FailureType.None);
	}

	[Theory]
	[ClassData(typeof(CanFailTestData.SingleErrorFailureTypes))]
	public void Type_Should_ReturnCorrectSingleFailureType_When_OneError(Error error, FailureType expectedFailureType)
	{
		//Arrange
		AbstractCanFail result = new();
		result.Failed(error);

		//Act
		var resultType = result.Type;

		//Assert
		resultType.Should().Be(expectedFailureType);
	}

	[Theory]
	[ClassData(typeof(CanFailTestData.MultipleErrorFailureTypes))]
	public void Type_Should_ReturnCorrectMultipleFailureType_When_MultipleErrorsOfOneType(Error error, FailureType expectedFailureType)
	{
		//Arrange
		AbstractCanFail result = new();
		result.Failed(error);
		result.Failed(error);

		//Act
		var resultType = result.Type;

		//Assert
		resultType.Should().Be(expectedFailureType);
	}

	[Fact]
	public void Type_Should_ReturnMultipleDifferent_When_DifferentErrors()
	{
		//Arrange
		AbstractCanFail result = new();
		result.Failed(Error.Conflict("", ""));
		result.Failed(Error.Validation("", ""));

		//Act
		var resultType = result.Type;

		//Assert
		resultType.Should().Be(FailureType.MultipleDifferent);
	}

	[Fact]
	public void DefaultConstructor_Should_BeSuccess()
	{
		//Act
		CanFail result = new();

		//Assert
		result.HasFailed.Should().BeFalse();
	}

	[Fact]
	public void Errors_Should_ThrowInvalidOperationException_When_NoErrors()
	{
		//Act
		CanFail result = new();

		//Assert
		result.Invoking(x => x.Errors).Should().Throw<InvalidOperationException>();
	}
}
