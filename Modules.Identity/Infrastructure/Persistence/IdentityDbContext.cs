using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Domain.Entities;
using SharedKernel.Outbox;

namespace Modules.Identity.Infrastructure.Persistence;

internal class IdentityDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<TenantSettings> TenantSettings => Set<TenantSettings>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("identity");

        builder.Entity<Tenant>(e =>
        {
            e.HasIndex(t => t.Slug).IsUnique();
            e.HasIndex(t => t.CustomDomain)
                .IsUnique()
                .HasFilter("\"CustomDomain\" IS NOT NULL");
        });

        builder.Entity<TenantSettings>()
            .HasKey(s => s.TenantId);

        builder.Entity<AppUser>(e =>
        {
            e.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
        });

        builder.Entity<RefreshToken>(e =>
        {
            e.HasIndex(t => t.TokenHash).IsUnique();
        });

        builder.Entity<OutboxMessage>(e =>
        {
            e.HasIndex(o => new { o.Status, o.NextAttemptAt });
        });
    }
}
