using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace Greentube.DemoWallet.Database.Services;

internal class WalletDbContextDesignTimeFactory : IDesignTimeDbContextFactory<WalletDbContext>
{
    public WalletDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WalletDbContext>();

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(args.FirstOrDefault());
        if (string.IsNullOrEmpty(connectionStringBuilder.Host))
        {
            connectionStringBuilder.Host = "localhost";
        }
        if (string.IsNullOrEmpty(connectionStringBuilder.Database))
        {
            connectionStringBuilder.Database = "wallet";
        }
        if (string.IsNullOrEmpty(connectionStringBuilder.Username))
        {
            connectionStringBuilder.Username = "postgres";
        }
        if (string.IsNullOrEmpty(connectionStringBuilder.Password))
        {
            connectionStringBuilder.Password = "psqldevpwd";
        }

        optionsBuilder.UseNpgsql(connectionStringBuilder.ConnectionString);

        return new WalletDbContext(optionsBuilder.Options);
    }
}
