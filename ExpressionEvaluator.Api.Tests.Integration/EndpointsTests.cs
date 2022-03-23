using ExpressionEvaluator.Api.Contracts;
using ExpressionEvaluator.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ExpressionEvaluator.Api.Tests.Integration;

public class EndpointsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<IApiMarker> _webApiFactory;

    public EndpointsTests(WebApplicationFactory<IApiMarker> webApiFactory)
    {
        _webApiFactory = webApiFactory;
    }

    [Fact]
    public async Task GetVariables_ReturnsEmptyArray_WhenNoVariableExists()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();

        // Act
        var result = await httpClient.GetAsync("/variables");
        var returnedVariables = await result.Content.ReadFromJsonAsync<List<MathVariableGetResponse>>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        returnedVariables.Should().BeEquivalentTo(new List<MathVariableGetResponse>()
        {
        });
    }

    [Fact]
    public async Task GetVariables_ReturnsAllVariables_WhenVariablesExist()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();

        await httpClient
            .PostAsJsonAsync("/variables", new MathVariableSetRequest("x", 1));

        // Act
        var result = await httpClient.GetAsync("/variables");
        var returnedVariables = await result.Content.ReadFromJsonAsync<List<MathVariableGetResponse>>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        returnedVariables.Should().BeEquivalentTo(new List<MathVariableGetResponse>()
        {
            new MathVariableGetResponse("x", 1),
        });
    }

    [Fact]
    public async Task SetVariable_ShouldCreateNewVariable_IfNotExistsYet()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();
        var newVariable = new MathVariableSetRequest("x", 1);

        // Act
        var result = await httpClient.PostAsJsonAsync("/variables", newVariable);
        var createdVariable = await result.Content.ReadFromJsonAsync<MathVariableSetRequest>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        createdVariable.Should().BeEquivalentTo(newVariable);
    }

    [Fact]
    public async Task SetVariable_ShouldUpdateVariable_IfAlreadyExists()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();
        var variable = new MathVariableSetRequest("x", 1);
        _ = await httpClient.PostAsJsonAsync("/variables", variable);

        // Act
        var newVariable = new MathVariableSetRequest("x", 2);
        var result = await httpClient.PostAsJsonAsync("/variables", newVariable);
        var createdVariable = await result.Content.ReadFromJsonAsync<MathVariableSetRequest>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        createdVariable.Should().BeEquivalentTo(newVariable);
    }

    [Fact]
    public async Task SetVariable_ShouldReturn400_WhenVariableNameIsInvalid()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();
        var variable = new MathVariableSetRequest("x1", 1);
        var expectedResponse = new ValidationError
        (
            propertyName: "Name",
            errorMessage: "Variable name is not valid! Not allowed digits and/or consecutive underscores __",
            attemptedValue: "x1",
            customState: null,
            severity: 0,
            errorCode: "RegularExpressionValidator",
            formattedMessagePlaceholderValues: new PlaceholderValues
            (
                RegularExpression: "^([a-z]+)([_][a-z]+)*$",
                PropertyName: "Name",
                PropertyValue: "x1"
            )
        );

        // Act
        var result = await httpClient.PostAsJsonAsync("/variables", variable);
        var response = await result.Content.ReadFromJsonAsync<List<ValidationError>>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Should().Contain(expectedResponse);
    }

    [Fact]
    public async Task Evaluate_ShouldReturnCorrectCalculation_WhenRequestedExpressionIsValid()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();
        var expression = new MathExpressionRequest("1+2*3");

        // Act
        var result = await httpClient.PostAsJsonAsync("/evaluate", expression);
        var returnedEvaluationResponse = await result.Content.ReadFromJsonAsync<MathEvaluationResponse>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        returnedEvaluationResponse.Should().BeEquivalentTo(new MathEvaluationResponse(expression.Expression, 7));
    }

    [Fact]
    public async Task Evaluate_ShouldReturnCorrectCalculation_WhenRequestedExpressionWithVariablesIsValid()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();
        var expression = new MathExpressionRequest("x+2*y");

        _ = await httpClient
            .PostAsJsonAsync("/variables", new MathVariableSetRequest("x", 1));
        _ = await httpClient
            .PostAsJsonAsync("/variables", new MathVariableSetRequest("y", 3));

        // Act
        var result = await httpClient.PostAsJsonAsync("/evaluate", expression);
        var returnedEvaluationResponse = await result.Content.ReadFromJsonAsync<MathEvaluationResponse>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        returnedEvaluationResponse.Should().BeEquivalentTo(new MathEvaluationResponse(expression.Expression, 7));
    }

    [Fact]
    public async Task Evaluate_ShouldReturn404_WhenRequestedExpressionIsValid_ButUsesUnknownVariables()
    {
        // Arrange
        var httpClient = _webApiFactory.CreateClient();
        var expression = new MathExpressionRequest("x+2*3");

        // Act
        var result = await httpClient.PostAsJsonAsync("/evaluate", expression);
        var returnedEvaluationResponse = await result.Content.ReadFromJsonAsync<VariableNotFoundExceptionResponse>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        returnedEvaluationResponse.Should().BeEquivalentTo(new VariableNotFoundExceptionResponse
        (
            Message: "No variable with name x was found",
            Variables: new List<MathVariable>()
        ));
    }

    // We don't care about InitializeAsync
    public Task InitializeAsync() => Task.CompletedTask;

    // Here we delete all created variables during tests in the storage
    // This is called after each Test function, not after all tests
    public async Task DisposeAsync()
    {
        var httpClient = _webApiFactory.CreateClient();
        await httpClient.DeleteAsync($"/variables"); // this endpoint deletes all
    }
}


// We cannot instantiate FluentValidation ValidationFailure objects
// so we manually create the expected object
internal record ValidationError(
    string propertyName,
    string errorMessage,
    string attemptedValue,
    string? customState,
    int severity,
    string errorCode,
    PlaceholderValues formattedMessagePlaceholderValues);

internal record PlaceholderValues(
    string RegularExpression,
    string PropertyName,
    string PropertyValue);