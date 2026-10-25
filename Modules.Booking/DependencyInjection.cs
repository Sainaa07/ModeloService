using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Booking.Infrastructure.Persistence;

namespace Modules.Booking;

public static class DependencyInjection
{
    public static IServiceCollection AddBookingModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(
                config.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "booking")
            )
        );
        services.AddHealthChecks().AddDbContextCheck<BookingDbContext>("booking-db");
        return services;
    }

    public static async Task MigrateAsync(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
        await db.Database.MigrateAsync();
    }
}
