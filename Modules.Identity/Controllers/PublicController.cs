using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Identity.Contracts;
using SharedKernel.Tenancy;

namespace Modules.Identity.Controllers;

[ApiController]
[Route("api/v1/public")]
[AllowAnonymous]
public class PublicController(ITenantSettingsReader settingsReader, ITenantContext tenantContext) : ControllerBase
{
    [HttpGet("tenant")]
    public async Task<IActionResult> GetTenant(CancellationToken ct)
    {
        if (tenantContext.TenantId is null)
            return NotFound();

        var settings = await settingsReader.GetAsync(tenantContext.TenantId.Value, ct);
        return Ok(settings);
    }
}
