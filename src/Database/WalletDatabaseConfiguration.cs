using System.ComponentModel.DataAnnotations;
using System.Data;
using Polly;

namespace Greentube.DemoWallet.Database;

public class WalletDatabaseConfiguration
{
    private static readonly IAsyncPolicy DefaultPolicy = Policy.NoOpAsync();

    [Required]
    public string Host { get; set; } = null!;

    [Required]
    public string User { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Database { get; set; } = null!;

    [Range(0, 100)]
    public int RetryAttemptsOnTransientError { get; set; } = 2;

    public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.Snapshot;

    [Required]
    public IAsyncPolicy ExecutionPolicy { get; set; } = DefaultPolicy;
}
