using System.Net;

namespace Greentube.DemoWallet.Application.Errors;

public class ApplicationErrorException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApplicationErrorException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
