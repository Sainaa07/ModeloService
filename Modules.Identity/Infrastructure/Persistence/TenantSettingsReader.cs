using Microsoft.EntityFrameworkCore;
using Modules.Identity.Contracts;

namespace Modules.Identity.Infrastructure.Persistence;

internal class TenantSettingsReader(IdentityDbContext db) : ITenantSettingsReader
{
    public async Task<TenantSettingsDto> GetAsync(Guid tenantId, CancellationToken ct = default)
    {
        var tenant = await db.Tenants
            .AsNoTracking()
            .Where(t => t.Id == tenantId)
            .Select(t => t.Name)
            .FirstOrDefaultAsync(ct)
            ?? throw new KeyNotFoundException($"Tenant {tenantId} not found");

        var settings = await db.TenantSettings
            .AsNoTracking()
            .Where(s => s.TenantId == tenantId)
            .FirstOrDefaultAsync(ct);

        return new TenantSettingsDto(
            Name: tenant,
            TimeZone: settings?.TimeZone ?? "Asia/Ulaanbaatar",
            Locale: settings?.Locale ?? "mn",
            CancelHoursBefore: settings?.CancelHoursBefore ?? 24,
            SlotStepMinutes: settings?.SlotStepMinutes ?? 30,
            AutoConfirm: settings?.AutoConfirm ?? false,
            LogoUrl: settings?.LogoUrl,
            ContactPhone: settings?.ContactPhone,
            ContactEmail: settings?.ContactEmail
        );
    }
}
