using CleanValidationMapper;
using CleanValidationMapperTests.TestData;
using FluentAssertions;
using Moq;
using System.Diagnostics;

namespace CleanValidationMapperTests;

public class CanFailOfTTests
{
	#region Definitions 

	private static void Succeeded_Should_SetValue_Helper<T>(T value)
	{
		//Arrange
		CanFail<T> result = new();

		//Act
		result.Succeded(value);

		//Assert
		result.HasFailed.Should().BeFalse();
		result.Value.Should().Be(value);
	}

	private static void Value_Should_ThrowInvalidOperationException_When_Errors_Helper<T>()
	{
		//Arrange
		CanFail<T> result = new();
		result.Failed(ErrorTestData.Error);

		//Act & Assert
		result.Invoking(x => x.Value).Should().Throw<InvalidOperationException>();
	}


	private static void Implicit_Should_ReturnInstanceWithValueFromValue_Helper<T>(T value)
	{
		//Act
		CanFail<T> result = value;

		//Assert
		result.HasFailed.Should().BeFalse();
		result.Value.Should().Be(value);
 	}

	private static void Implicit_Should_ReturnInstanceWithErrorFromError_Helper<T>()
	{
		//Arrange
		Error error = ErrorTestData.Error;

		//Act
		CanFail<T> result = error;

		//Assert
		result.HasFailed.Should().BeTrue();
		result.Errors.Should().ContainSingle().Which.Should().Be(error);
	}

	private static void DefaultConstructor_Should_BeFailure_Helper<T>()
	{
		//Act
		CanFail<T> result = new();

		//Assert
		result.Invoking(x => x.Value).Should().Throw<InvalidOperationException>();
	}

	#endregion

	#region Tests

	[Fact]
	public void Succeeded_Should_SetValue()
	{
		Succeeded_Should_SetValue_Helper("Success");
		Succeeded_Should_SetValue_Helper(42);
	}

	[Fact]
	public void Value_Should_ThrowInvalidOperationException_When_Errors()
	{
		Value_Should_ThrowInvalidOperationException_When_Errors_Helper<string>();
		Value_Should_ThrowInvalidOperationException_When_Errors_Helper<int>();
	}

	[Fact]
	public void Implicit_Should_ReturnInstanceWithValueFromValue()
	{
		Implicit_Should_ReturnInstanceWithValueFromValue_Helper("Success");
		Implicit_Should_ReturnInstanceWithValueFromValue_Helper((string?) null);
		Implicit_Should_ReturnInstanceWithValueFromValue_Helper(42);
		Implicit_Should_ReturnInstanceWithValueFromValue_Helper((int?) null);
	}

	[Fact]
	public void Implicit_Should_ReturnInstanceWithErrorFromError()
	{
		Implicit_Should_ReturnInstanceWithErrorFromError_Helper<string>();
		Implicit_Should_ReturnInstanceWithErrorFromError_Helper<int>();
	}

	[Fact]
	public void DefaultConstructor_Should_BeFailure()
	{
		DefaultConstructor_Should_BeFailure_Helper<string>();
		DefaultConstructor_Should_BeFailure_Helper<int>();
	}

	#endregion
}
