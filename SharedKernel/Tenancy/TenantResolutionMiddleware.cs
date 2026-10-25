using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SharedKernel.Tenancy;

public class TenantResolutionMiddleware(
    RequestDelegate next,
    IConfiguration config)
{
    public async Task InvokeAsync(
        HttpContext ctx,
        ITenantStore store,
        IMemoryCache cache,
        ITenantContext tenantCtx)
    {
        // 1. Хүсэлт ирсэн hostname
        var host = ctx.Request.Host.Host.ToLowerInvariant();

        // 2. Platform hostname
        var platformHost = (config["Platform:Host"] ?? "localhost")
            .ToLowerInvariant();

        // 3. Development орчинд X-Tenant header ашиглаж болно (platform check-ийн өмнө)
        string? slug = null;

        var environment = ctx.RequestServices
            .GetRequiredService<IHostEnvironment>();

        if (environment.IsDevelopment())
        {
            slug = ctx.Request.Headers["X-Tenant"]
                .FirstOrDefault()
                ?.Trim()
                .ToLowerInvariant();
        }

        // 4. Platform хүсэлт (X-Tenant header байхгүй үед)
        if (string.IsNullOrWhiteSpace(slug) && host == platformHost)
        {
            ((TenantContext)tenantCtx).SetPlatform();

            await next(ctx);
            return;
        }

        // 5. Subdomain-оос slug авах
        slug = string.IsNullOrWhiteSpace(slug)
            ? host.Split('.')[0]
            : slug;

        // 6. Cache key
        var cacheKey = $"tenant:{host}:{slug}";

        // 7. Tenant хайх
        if (!cache.TryGetValue(cacheKey, out TenantLookupResult? tenant))
        {
            tenant = await store.FindBySlugAsync(slug);

            if (tenant is null)
            {
                tenant = await store.FindByDomainAsync(host);
            }

            if (tenant is not null)
            {
                cache.Set(
                    cacheKey,
                    tenant,
                    TimeSpan.FromMinutes(5)
                );
            }
        }

        // 8. Tenant олдоогүй
        if (tenant is null)
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        // 9. Tenant идэвхгүй
        if (tenant.Status == "Suspended")
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        // 10. Tenant context тохируулах
        ((TenantContext)tenantCtx).Set(
            tenant.TenantId,
            tenant.Slug
        );

        // 11. Дараагийн middleware рүү шилжих
        await next(ctx);
    }
}