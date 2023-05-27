using Greentube.DemoWallet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Greentube.DemoWallet.Database.Services;

internal class WalletDbContext : DbContext
{
    public DbSet<Player> Players { get; init; } = null!;
    public DbSet<Transaction> Transactions { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var playerEntity = modelBuilder.Entity<Player>();
        playerEntity.ToTable(nameof(Player));
        playerEntity.HasKey(x => x.Id);
        playerEntity.HasMany(x => x.Transactions).WithOne().HasForeignKey(x => x.PlayerId);

        var transactionEntity = modelBuilder.Entity<Transaction>();
        transactionEntity.ToTable(nameof(Transaction));
        transactionEntity.HasKey(x => x.Id);
        transactionEntity.HasIndex(x => x.CreatedOn);
        transactionEntity.HasIndex(x => new { x.Type, x.CreatedOn });
    }

    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
    {
    }

    // dotnet tool update dotnet-ef -g
    // dotnet ef migrations add MigrationName -p src/Database/Database.csproj --msbuildprojectextensionspath artifacts/obj/Database -- [ConnectionString optional]
}
