using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskFlow.Web.Filters;

public class ValidationExceptionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var executedContext = await next();

        if (executedContext.Exception is not ValidationException exception)
        {
            return;
        }

        foreach (var error in exception.Errors)
        {
            context.ModelState.AddModelError(
                error.PropertyName,
                error.ErrorMessage);
        }

        var model = context.ActionArguments.Values.FirstOrDefault();

        executedContext.Result = new ViewResult
        {
            ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                context.ModelState)
            {
                Model = model
            }
        };

        executedContext.ExceptionHandled = true;
    }
}