using ExpressionEvaluator.Api.Contracts;
using ExpressionEvaluator.Api.Exceptions;
using ExpressionEvaluator.Api.Models;
using ExpressionEvaluator.Api.Services;
using ExpressionEvaluator.Api.Services.Preprocessors;
using ExpressionEvaluator.Api.Services.Storages;
using ExpressionEvaluator.Api.Validators;
using FluentValidation;
using FluentValidation.Results;
using MathExpression.Library;
using MathExpression.Library.Exceptions;
using MathExpression.Library.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IValidator<MathExpressionRequest>, MathExpressionValidator>();
builder.Services.AddScoped<IValidator<MathVariableSetRequest>, MathVariableSetRequestValidator>();

builder.Services.AddScoped<IMathExpressionService, MathExpressionService>();

builder.Services.AddScoped<IMathExpressionTokenizer, MathExpressionTokenizer>();
builder.Services.AddScoped<IMathExpressionConverter, MathExpressionConverter>();
builder.Services.AddScoped<IMathExpressionEvaluator, MathExpressionEvaluator>();

builder.Services.AddScoped<IMathExpressionPreprocessorService, MathExpressionPreprocessorService>();

builder.Services.AddSingleton<IMathVariablesStorageService, MathVariablesStorageService>();

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

// You can handle middleware errors
// More info 1: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-6.0#exception-handler-lambda
// More info 2: https://andrewlock.net/creating-a-custom-error-handler-middleware-function/
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        // This example avoids sending the user the full 500 stacktrace
        if (exceptionHandlerPathFeature?.Endpoint?.DisplayName == "SetVariable" &&
            exceptionHandlerPathFeature?.Error.GetBaseException() is System.FormatException)
        {
            await context.Response
                .WriteAsJsonAsync(new
                {
                    errorMessage = "JSON body has a wrong format. Please, check swagger docs for more info."
                });
        }
    });
});


app.UseHttpsRedirection();


app.MapPost("/evaluate",
    async (MathExpressionRequest mathExpressionRequest,
    IValidator<MathExpressionRequest> validator,
    IMathExpressionPreprocessorService mathExpressionPreprocessorService,
    IMathExpressionService mathExpressionService,
    IMathVariablesStorageService mathVariablesStorageService) =>
    {

        // Validate
        var validationResult = await validator.ValidateAsync(mathExpressionRequest);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        // Processing input before send to evaluation
        var expression = mathExpressionRequest.Expression;
        try
        {
            expression = await mathExpressionPreprocessorService
            .AssignVariablesToMathExpressionAsync(mathExpressionRequest.Expression);
        }
        catch (VariableNotFoundException ex)
        {
            return Results.NotFound(new VariableNotFoundExceptionResponse(ex.Message, ex.ExistingVariables));
        }

        // Evaluate
        var result = 0.0;
        try
        {
            // we need to pass to mathExpressionService all data in plain numbers
            // so cannot be mathExpressionRequest directly (1+x+2) => (1+6+2)
            result = mathExpressionService.Eval(expression);
        }
        catch (InvalidMathExpressionException e)
        {
            return Results.BadRequest(e.Message);
        }

        // Return the result
        return Results.Ok(new MathEvaluationResponse(Expression: mathExpressionRequest.Expression, Result: result));
    })
    .Accepts<MathExpressionRequest>("application/json")
    .Produces<IEnumerable<ValidationFailure>>(400)
    .Produces<VariableNotFoundExceptionResponse>(404)
    .Produces<string>(400)
    .Produces<MathEvaluationResponse>(200);


// TODO: if x = 0, it says variable value is not defined
app.MapPost("/variables", async (
    MathVariableSetRequest mathVariableSetRequest,
    IValidator<MathVariableSetRequest> validator,
    IMathVariablesStorageService mathVariablesStorageService) =>
    {
        // Some validations
        var validationResult = await validator.ValidateAsync(mathVariableSetRequest);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        // Create the variable model
        MathVariable mathVariable = new(mathVariableSetRequest.Name, mathVariableSetRequest.Value);

        // Store variable
        await mathVariablesStorageService.CreateAsync(mathVariable);

        return Results.Ok(mathVariable);
    })
    .WithDisplayName("SetVariable")
    .Accepts<MathVariableSetRequest>("application/json")
    .Produces<string>(200)
    .Produces<IEnumerable<ValidationFailure>>(400);


app.MapGet("/variables",
    async (IMathVariablesStorageService mathVariablesStorageService) =>
    {
        var allVariables = await mathVariablesStorageService.GetAllAsync();
        return Results.Ok(allVariables);
    })
    .Produces<IEnumerable<MathVariableGetResponse>>(200);

app.Run();


