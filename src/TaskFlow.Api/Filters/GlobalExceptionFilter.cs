using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskFlow.Application.Common.Exceptions;

namespace TaskFlow.Api.Filters;

public sealed class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            ValidationException exception => CreateValidationResult(exception),
            NotFoundException exception => new NotFoundObjectResult(
                new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found",
                    Detail = exception.Message
                }),
            _ => null
        };

        context.ExceptionHandled = context.Result is not null;
    }

    private static BadRequestObjectResult CreateValidationResult(
        ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .ToArray());

        return new BadRequestObjectResult(
            new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed"
            });
    }
}
