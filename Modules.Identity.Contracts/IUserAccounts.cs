namespace Modules.Identity.Contracts;

public interface IUserAccounts
{
    Task<Guid> CreateStaffUserAsync(Guid tenantId, string email, string fullName, CancellationToken ct = default);
}
