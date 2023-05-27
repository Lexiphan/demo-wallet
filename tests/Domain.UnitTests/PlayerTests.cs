namespace Greentube.DemoWallet.Domain.UnitTests;

public class PlayerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void TopUp_ShouldAddBalance_AndTopUpTransaction()
    {
        var player = _fixture.Create<Player>();
        var amount = (decimal)(Random.Shared.NextDouble() * 1000);

        player.TopUp(amount);

        player.Balance.Should().Be(amount);
        player.Transactions.Should().Satisfy(t => t.Amount == amount && t.Type == TransactionType.TopUp);
    }

    [Fact]
    public void Bet_ShouldSubtractBalance_AndAddBetTransaction()
    {
        var player = _fixture.Create<Player>();
        player.BalanceLimit = -1000;
        var amount = (decimal)(Random.Shared.NextDouble() * 1000);

        var result = player.Bet(amount);

        result.Should().BeTrue();
        player.Balance.Should().Be(-amount);
        player.Transactions.Should().Satisfy(t => t.Amount == -amount && t.Type == TransactionType.Bet);
    }

    [Fact]
    public void Bet_NoEnoughBalance_ShouldReturnFalse()
    {
        var player = _fixture.Create<Player>();
        var amount = (decimal)(Random.Shared.NextDouble() * 1000);
        player.BalanceLimit = -amount + 1;

        var result = player.Bet(amount);

        result.Should().BeFalse();
        player.Balance.Should().Be(0);
        player.Transactions.Should().BeEmpty();
    }

    [Fact]
    public void Win_ShouldAddBalanceAndWinTransaction()
    {
        var player = _fixture.Create<Player>();
        var amount = (decimal)(Random.Shared.NextDouble() * 1000);

        player.Win(amount);

        player.Balance.Should().Be(amount);
        player.Transactions.Should().Satisfy(t => t.Amount == amount && t.Type == TransactionType.Win);
    }
}
