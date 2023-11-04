using CleanValidationMapper;
using CleanValidationMapperTests.TestData;
using FluentAssertions;

namespace CleanValidationMapperTests;

public class CanFailTests
{
	[Theory]
	[ClassData(typeof(CanFailTestData.ErrorLists))]
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
	public void Failed_Should_AddResults(List<CanFail> results)
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

	private static void ValidateErrorsSetCorrectly(Error inList, Error original)
	{
		inList.Should().Be(original);
	}

	[Fact]
	public void DefaultConstructorIsSuccess()
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

	[Fact]
	public void Implicit_Should_ReturnInstanceWithErrorFromError()
	{
		//Arrange
		Error error = CanFailTestData.Error;

		//Act
		CanFail result = error;

		//Assert
		result.HasFailed.Should().BeTrue();
		result.Errors.Should().ContainSingle().Which.Should().Be(error);
	}
}
