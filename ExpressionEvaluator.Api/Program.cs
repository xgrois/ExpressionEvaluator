using ExpressionEvaluator.Api.Contracts;
using ExpressionEvaluator.Api.Services;
using ExpressionEvaluator.Api.Validators;
using FluentValidation;
using FluentValidation.Results;
using MathExpression.Library;
using MathExpression.Library.Exceptions;
using MathExpression.Library.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IValidator<MathExpressionRequest>, MathExpressionValidator>();

builder.Services.AddSingleton<IMathExpressionService, MathExpressionService>();

builder.Services.AddSingleton<IMathExpressionTokenizer, MathExpressionTokenizer>();
builder.Services.AddSingleton<IMathExpressionConverter, MathExpressionConverter>();
builder.Services.AddSingleton<IMathExpressionEvaluator, MathExpressionEvaluator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/evaluate",
    async (HttpContext httpContext, MathExpressionRequest mathExpressionRequest, IValidator<MathExpressionRequest> validator, IMathExpressionService mathExpressionService) =>
    {

        // Validate
        var validationResult = await validator.ValidateAsync(mathExpressionRequest);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        // Evaluate
        var result = 0.0;
        try
        {
            result = mathExpressionService.Eval(mathExpressionRequest.Expression);
        }
        catch (InvalidMathExpressionException e)
        {
            return Results.BadRequest(e.Message);
        }


        // Return the result
        return Results.Ok(new MathEvaluationResponse(Expression: mathExpressionRequest.Expression, Result: result));
    })
    .WithName("EvaluateExpressionPOST")
    .Accepts<MathExpressionRequest>("application/json")
    .Produces<IEnumerable<ValidationFailure>>(400)
    .Produces<MathEvaluationResponse>(200);



app.Run();


