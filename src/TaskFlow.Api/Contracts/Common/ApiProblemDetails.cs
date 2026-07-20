using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Contracts.Common;

public static class ApiProblemDetails
{
    public static ProblemDetails NotFound(string detail)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Recurso não encontrado",
            Detail = detail,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5"
        };
    }
}
