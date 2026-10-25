namespace Modules.Identity.Domain.Entities;

public class TenantSettings
{
    public Guid TenantId { get; set; }
    public string? LogoUrl { get; set; }
    public string TimeZone { get; set; } = "Asia/Ulaanbaatar";
    public string Locale { get; set; } = "mn";
    public int CancelHoursBefore { get; set; } = 24;
    public int SlotStepMinutes { get; set; } = 30;
    public bool AutoConfirm { get; set; } = false;
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
}
