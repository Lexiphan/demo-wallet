namespace Greentube.DemoWallet.Domain;

public class Entity
{
    public long Id { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public Entity(DateTime? createdOn = null)
    {
        CreatedOn = createdOn ?? DateTime.UtcNow;
    }
}
