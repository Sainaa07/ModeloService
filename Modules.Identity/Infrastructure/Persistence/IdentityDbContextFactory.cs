using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Modules.Identity.Infrastructure.Persistence;

internal class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=Booking_dev;Username=postgres;Password=1217",
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "identity")
            )
            .Options;

        return new IdentityDbContext(options);
    }
}
