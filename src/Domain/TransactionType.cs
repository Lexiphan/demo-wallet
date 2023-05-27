namespace Greentube.DemoWallet.Domain;

public enum TransactionType
{
    /// <summary>
    /// The player topped his balance up
    /// </summary>
    TopUp = 1,

    /// <summary>
    /// The player made a bet. Amount in this case is negative
    /// </summary>
    Bet = 2,

    /// <summary>
    /// The player won the game and his balance was increased
    /// </summary>
    Win = 3,
}
