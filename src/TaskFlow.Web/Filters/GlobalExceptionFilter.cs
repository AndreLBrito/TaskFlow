using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using TaskFlow.Application.Common.Exceptions;

namespace TaskFlow.Web.Filters;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        Log.Error(
            context.Exception,
            "Erro não tratado");

        switch (context.Exception)
        {
            case BusinessRuleException businessRuleException:
                context.Result = new BadRequestObjectResult(
                    new
                    {
                        Error = businessRuleException.Message
                    });

                context.ExceptionHandled = true;
                break;

            case NotFoundException exception:
                context.Result = new NotFoundObjectResult(
                    new { Error = exception.Message });
                context.ExceptionHandled = true;
                break;

            default:
                break;
        }

        return Task.CompletedTask;
    }
}