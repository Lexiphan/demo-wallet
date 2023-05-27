namespace Greentube.DemoWallet.Domain;

public class Transaction : Entity
{
    public long PlayerId { get; private set; }

    public TransactionType Type { get; private set; }

    public decimal Amount { get; private set; }

    internal Transaction(Player player, TransactionType type, decimal amount, DateTime? createdOn = null)
        : base(createdOn)
    {
        Amount = type == TransactionType.Bet ? amount.EnsureLessThan(0) : amount.EnsureGreaterThan(0);
        Type = type;
        PlayerId = player.Id;
    }

    /// <summary>
    /// This constructor is used by EF
    /// </summary>
    private Transaction()
    {
    }
}
