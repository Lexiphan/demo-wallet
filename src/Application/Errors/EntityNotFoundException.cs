using System.Net;

namespace Greentube.DemoWallet.Application.Errors;

public class EntityNotFoundException : ApplicationErrorException
{
    public EntityNotFoundException(Type entityType, long id)
        : base($"Resource '{entityType.Name}' with id {id} was not found", HttpStatusCode.NotFound)
    {
    }
}
