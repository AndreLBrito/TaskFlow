using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskFlow.Application.Common.Exceptions;

namespace TaskFlow.Api.Filters;

public sealed class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            ValidationException exception => CreateValidationResult(exception),
            NotFoundException exception => CreateProblemResult(
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                exception.Message),
            BusinessRuleException exception => CreateProblemResult(
                StatusCodes.Status400BadRequest,
                "Regra de negócio inválida",
                exception.Message),
            _ => CreateUnexpectedErrorResult(context.Exception)
        };

        context.ExceptionHandled = true;
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
                Title = "Erro de validação",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            });
    }

    private static ObjectResult CreateProblemResult(
        int status,
        string title,
        string detail)
    {
        return new ObjectResult(
            new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Type = status == StatusCodes.Status404NotFound
                    ? "https://tools.ietf.org/html/rfc9110#section-15.5.5"
                    : "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            })
        {
            StatusCode = status
        };
    }

    private ObjectResult CreateUnexpectedErrorResult(Exception exception)
    {
        _logger.LogError(exception, "Erro inesperado ao processar a requisição.");

        return CreateProblemResult(
            StatusCodes.Status500InternalServerError,
            "Erro interno do servidor",
            "Ocorreu um erro inesperado ao processar a requisição.");
    }
}
