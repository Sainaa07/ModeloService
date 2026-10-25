using Microsoft.EntityFrameworkCore;

namespace Modules.Booking.Infrastructure.Persistence;

internal class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("booking");
    }
}
