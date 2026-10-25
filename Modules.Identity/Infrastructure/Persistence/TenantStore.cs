using Microsoft.EntityFrameworkCore;
using SharedKernel.Tenancy;

namespace Modules.Identity.Infrastructure.Persistence;

internal class TenantStore(IdentityDbContext db) : ITenantStore
{
    public async Task<TenantLookupResult?> FindBySlugAsync(string slug, CancellationToken ct = default)
    {
        var tenant = await db.Tenants
            .AsNoTracking()
            .Where(t => t.Slug == slug)
            .Select(t => new { t.Id, t.Slug, t.Status })
            .FirstOrDefaultAsync(ct);

        return tenant is null ? null : new TenantLookupResult(tenant.Id, tenant.Slug, tenant.Status);
    }

    public async Task<TenantLookupResult?> FindByDomainAsync(string domain, CancellationToken ct = default)
    {
        var tenant = await db.Tenants
            .AsNoTracking()
            .Where(t => t.CustomDomain == domain)
            .Select(t => new { t.Id, t.Slug, t.Status })
            .FirstOrDefaultAsync(ct);

        return tenant is null ? null : new TenantLookupResult(tenant.Id, tenant.Slug, tenant.Status);
    }
}
