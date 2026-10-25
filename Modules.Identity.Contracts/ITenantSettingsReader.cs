namespace Modules.Identity.Contracts;

public interface ITenantSettingsReader
{
    Task<TenantSettingsDto> GetAsync(Guid tenantId, CancellationToken ct = default);
}

public record TenantSettingsDto(
    string Name,
    string TimeZone,
    string Locale,
    int CancelHoursBefore,
    int SlotStepMinutes,
    bool AutoConfirm,
    string? LogoUrl,
    string? ContactPhone,
    string? ContactEmail
);
