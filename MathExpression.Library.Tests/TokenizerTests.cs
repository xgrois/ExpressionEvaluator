using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace MathExpression.Library.Tests.Unit;

public class TokenizerTests
{
    private readonly MathExpressionTokenizer _sut = new();

    [Theory]
    [ClassData(typeof(ValidMathExpressionsForTest))]
    public void Tokenizer_ShouldTokenize_WhenInputExpressionIsValid(List<Token> expectedTokens, string inputExpression)
    {
        // Arrage

        // Act
        var tokens = _sut.Tokenize(inputExpression);

        // Assert
        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Theory]
    [ClassData(typeof(ValidMathExpressionsWithSpacesForTest))]
    public void Tokenizer_ShouldTokenize_WhenInputExpressionIsValidWithSpaces(string inputExpression)
    {
        // Arrage
        var expectedTokens = new List<Token>
        {
            new Token() {TokenType = TokenType.Integer, Value = 10 },
            new Token() {TokenType = TokenType.Sum, Value = "+" },
            new Token() {TokenType = TokenType.Integer, Value = 20 },
            new Token() {TokenType = TokenType.Sub, Value = "-" },
            new Token() {TokenType = TokenType.Integer, Value = 5 },
            //new Token() {TokenType = TokenType.EndOfExpression, Value = null },
        };

        // Act
        var tokens = _sut.Tokenize(inputExpression);

        // Assert
        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Fact]
    public void Tokenizer_ShouldThrowException_WhenInputExpressionIsInvalid()
    {
        // Arrage
        var expectedTokens = new List<Token>(); // we dont care

        // Act
        var requestedAction = () => _sut.Tokenize("2.3");

        // Assert
        requestedAction.Should().Throw<Exception>().WithMessage($"Unknown operand .");
    }
}

public class ValidMathExpressionsForTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 10 },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
                new Token() {TokenType = TokenType.Integer, Value = 20 },
                new Token() {TokenType = TokenType.Sub, Value = "-" },
                new Token() {TokenType = TokenType.Integer, Value = 5 },
                //new Token() {TokenType = TokenType.EndOfExpression, Value = null },
            },
            "10+20-5"
        };
        yield return new object[]
        {
            new List<Token>
            {
                new Token() {TokenType = TokenType.Sub, Value = "-" },
                new Token() {TokenType = TokenType.Integer, Value = 10 },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
                new Token() {TokenType = TokenType.Integer, Value = 20 },
                new Token() {TokenType = TokenType.Sub, Value = "-" },
                new Token() {TokenType = TokenType.Integer, Value = 5 },
                //new Token() {TokenType = TokenType.EndOfExpression, Value = null },
            },
            "-10+20-5"
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ValidMathExpressionsWithSpacesForTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { " 10+20-5" };
        yield return new object[] { " 10 +20-5" };
        yield return new object[] { " 10 + 20-5" };
        yield return new object[] { " 10 + 20 -5" };
        yield return new object[] { " 10 + 20 - 5" };
        yield return new object[] { " 10 + 20 - 5 " };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}