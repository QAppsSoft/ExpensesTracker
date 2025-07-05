using System.Net;
using Api.Models;

namespace ApiFixtures;

public class ApiResponseTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateSuccess_WithValidResult_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var testData = new { Value = 42 };
        
        // Act
        var response = APIResponse.CreateSuccess(testData, HttpStatusCode.Created);
        
        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Result.Should().Be(testData);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.ErrorMessages.Should().BeEmpty();
    }

    [Test]
    public void CreateSuccess_WithNullResult_ShouldThrowArgumentNullException()
    {
        // Arrange & Act
        var action = () => APIResponse.CreateSuccess(null!);
        
        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Test]
    public void CreateError_WithErrorMessage_ShouldReturnErrorResponse()
    {
        // Arrange
        var statusCode = HttpStatusCode.NotFound;
        
        // Act
        var response = APIResponse.CreateError(statusCode, "Not found", "Invalid ID");
        
        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Result.Should().BeNull();
        response.StatusCode.Should().Be(statusCode);
        response.ErrorMessages.Should()
            .ContainInOrder("Not found", "Invalid ID")
            .And.HaveCount(2);
    }

    [Test]
    public void CreateError_WithNoMessages_ShouldReturnEmptyErrorList()
    {
        // Act
        var response = APIResponse.CreateError(HttpStatusCode.BadRequest);
        
        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Result.Should().BeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.ErrorMessages.Should().BeEmpty();
    }

    [Test]
    public void CreateError_WithNullMessages_ShouldHandleNullGracefully()
    {
        // Act
        var response = APIResponse.CreateError(HttpStatusCode.InternalServerError, null!);
        
        // Assert
        response.ErrorMessages.Should().BeEmpty();
    }
}