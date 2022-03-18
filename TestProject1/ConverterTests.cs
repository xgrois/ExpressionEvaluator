using FluentAssertions;
using MathExpressionLibrary;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace MathExpression.Library.Tests.Unit;
public class ConverterTests
{
    private readonly MathExpressionConverter _sut = new();

    [Theory]
    [ClassData(typeof(ValidInfixTokenExpressionsForTest))]
    public void Converter_ShouldConvertTokenizedInfixExpressionToTokenizedPostfixExpression_WhenInputIsValid(List<Token> tokenizedInfixExpression, List<Token> expectedTokenizedPostfixExpression)
    {
        // Arrage

        // Act
        var tokens = _sut.InfixToPostfix(tokenizedInfixExpression);

        // Assert
        tokens.Should().BeEquivalentTo(expectedTokenizedPostfixExpression);
    }
}


public class ValidInfixTokenExpressionsForTest : IEnumerable<object[]>
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
            },
            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 10 },
                new Token() {TokenType = TokenType.Integer, Value = 20 },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
            },
        };
        yield return new object[]
        {

            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 1 },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
                new Token() {TokenType = TokenType.Integer, Value = 2 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Integer, Value = 3 },
                new Token() {TokenType = TokenType.Sub, Value = "-" },
                new Token() {TokenType = TokenType.Integer, Value = 4 },
                new Token() {TokenType = TokenType.Div, Value = "/" },
                new Token() {TokenType = TokenType.Integer, Value = 5 },
            },
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
        };
        yield return new object[]
{
            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 1 },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
                new Token() {TokenType = TokenType.LeftParenthesis, Value = "(" },
                new Token() {TokenType = TokenType.Integer, Value = 2 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Integer, Value = 3 },
                new Token() {TokenType = TokenType.Sub, Value = "-" },
                new Token() {TokenType = TokenType.LeftParenthesis, Value = "(" },
                new Token() {TokenType = TokenType.Integer, Value = 4 },
                new Token() {TokenType = TokenType.Div, Value = "/" },
                new Token() {TokenType = TokenType.Integer, Value = 5 },
                new Token() {TokenType = TokenType.Exp, Value = "^" },
                new Token() {TokenType = TokenType.Integer, Value = 6 },
                new Token() {TokenType = TokenType.RightParenthesis, Value = ")" },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Integer, Value = 7 },
                new Token() {TokenType = TokenType.RightParenthesis, Value = ")" },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Integer, Value = 8 },
            },
            new List<Token>
            {
                new Token() {TokenType = TokenType.Integer, Value = 1 },
                new Token() {TokenType = TokenType.Integer, Value = 2 },
                new Token() {TokenType = TokenType.Integer, Value = 3 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Integer, Value = 4 },
                new Token() {TokenType = TokenType.Integer, Value = 5 },
                new Token() {TokenType = TokenType.Integer, Value = 6 },
                new Token() {TokenType = TokenType.Exp, Value = "^" },
                new Token() {TokenType = TokenType.Div, Value = "/" },
                new Token() {TokenType = TokenType.Integer, Value = 7 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Sub, Value = "-" },
                new Token() {TokenType = TokenType.Integer, Value = 8 },
                new Token() {TokenType = TokenType.Mul, Value = "*" },
                new Token() {TokenType = TokenType.Sum, Value = "+" },
            },
};
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
