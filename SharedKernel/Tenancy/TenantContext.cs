namespace SharedKernel.Tenancy;

public class TenantContext : ITenantContext
{
    public Guid? TenantId { get; private set; }
    public string Slug { get; private set; } = string.Empty;
    public bool IsPlatform { get; private set; }

    public void Set(Guid tenantId, string slug)
    {
        TenantId = tenantId;
        Slug = slug;
        IsPlatform = false;
    }

    public void SetPlatform()
    {
        TenantId = null;
        Slug = string.Empty;
        IsPlatform = true;
    }
}
