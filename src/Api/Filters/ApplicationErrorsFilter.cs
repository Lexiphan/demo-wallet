using Greentube.DemoWallet.Application.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Greentube.DemoWallet.Api.Filters;

internal class ApplicationErrorsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ApplicationErrorException e:
                context.Result =
                    new ObjectResult(
                        new ProblemDetails
                        {
                            Detail = e.Message,
                            Status = (int)e.StatusCode,
                            Title = e.GetType().Name,
                        })
                    {
                        StatusCode = (int)e.StatusCode,
                    };
                break;
        }
    }
}
