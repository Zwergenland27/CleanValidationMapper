using CleanValidationMapper;
using CleanValidationMapper.Errors;
using CleanValidationMapperTests.TestData;
using FluentAssertions;
using Moq;

namespace CleanValidationMapperTests;

public class CanFailTests
{
    [Fact]
    public void Implicit_Should_ReturnInstanceWithErrorFromError()
    {
        //Arrange
        Error error = ErrorTestData.Error;

        //Act
        CanFail result = error;

        //Assert
        result.HasFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
