using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Tenancy
{
    public interface ITenantStore
    {
        Task<TenantLookupResult?> FindBySlugAsync(string slug, CancellationToken ct = default);
        Task<TenantLookupResult?> FindByDomainAsync(string domain, CancellationToken ct = default);
    }
    public record TenantLookupResult(Guid TenantId, string Slug, string Status);
}
