namespace Greentube.DemoWallet.Domain;

public class Player : Entity
{
    public string Name { get; set; } = null!;

    public decimal BalanceLimit { get; set; }

    public decimal Balance { get; private set; }

    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    public bool HasSufficientBalance(decimal amount) => Balance - amount.EnsureGreaterThan(0) >= BalanceLimit;

    public bool Bet(decimal amount, DateTime? utcNow = null)
    {
        if (!HasSufficientBalance(amount))
        {
            return false;
        }

        Balance -= amount;
        Transactions.Add(new Transaction(this, TransactionType.Bet, -amount, utcNow));
        return true;
    }

    public void Win(decimal amount, DateTime? utcNow = null)
    {
        Balance += amount.EnsureGreaterThan(0);
        Transactions.Add(new Transaction(this, TransactionType.Win, amount, utcNow));
    }

    public void TopUp(decimal amount, DateTime? utcNow = null)
    {
        Balance += amount.EnsureGreaterThan(0);
        Transactions.Add(new Transaction(this, TransactionType.TopUp, amount, utcNow));
    }

    public Player(string name, decimal balanceLimit, DateTime? createdOn = null) : base(createdOn)
    {
        Name = name;
        BalanceLimit = balanceLimit;
    }

    /// <summary>
    /// This constructor is used by EF.
    /// </summary>
    private Player()
    {
    }
}
