using FluentAssertions;
using MathExpressionLibrary;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace MathExpression.Library.Tests.Unit;
public class EvaluatorTests
{
    private readonly MathExpressionEvaluator _sut = new();
    private const double DELTA = 0.001;

    [Theory]
    [ClassData(typeof(ValidPostfixTokenExpressionsForTest))]
    public void Evaluator_ShouldEvalTokenizedPostfixExpression_WhenInputIsValid(List<Token> tokenizedPostfixExpression, double expectedResult)
    {
        // Arrage

        // Act
        var result = _sut.Eval(tokenizedPostfixExpression);

        // Assert
        result.Should().BeApproximately(expectedResult, DELTA); // expectedResult +- delta
    }
}

public class ValidPostfixTokenExpressionsForTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {

        yield return new object[]
        {
            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 10 },
                new Token() {TokenType = TokenType.Integer, Value = 20 },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
            },
            30
        };

        yield return new object[]
        {
            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 1 },
                new Token() {TokenType = TokenType.Integer, Value = 2 },
                new Token() {TokenType = TokenType.Integer, Value = 3 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
                new Token() {TokenType = TokenType.Integer, Value = 4 },
                new Token() {TokenType = TokenType.Integer, Value = 5 },
                new Token() {TokenType = TokenType.Div, Value = "/" },
                new Token() {TokenType = TokenType.Sub, Value = "-" },
            },
            6.2
        };

        yield return new object[]
        {
            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 1 },
                new Token() {TokenType = TokenType.Integer, Value = 2 },
                new Token() {TokenType = TokenType.Integer, Value = 3 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Integer, Value = 4 },
                new Token() {TokenType = TokenType.Integer, Value = 5 },
                new Token() {TokenType = TokenType.Integer, Value = 1 },
                new Token() {TokenType = TokenType.Exp, Value = "^" },
                new Token() {TokenType = TokenType.Div, Value = "/" },
                new Token() {TokenType = TokenType.Integer, Value = 2 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Sub, Value = "-" },
                new Token() {TokenType = TokenType.Integer, Value = 3 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
            },
            14.2
        };

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
