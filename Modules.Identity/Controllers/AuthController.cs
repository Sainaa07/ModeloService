using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Identity.Application.Services;
using SharedKernel.Tenancy;

namespace Modules.Identity.Controllers;

[ApiController]
[Route("api/v1/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ITenantContext _tenantContext;

    internal AuthController(UserService userService, ITenantContext tenantContext)
    {
        _userService = userService;
        _tenantContext = tenantContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (_tenantContext.TenantId is null)
            return Unauthorized(new { message = "Tenant олдсонгүй" });

        var result = await _userService.LoginAsync(
            request.Email, request.Password, _tenantContext.TenantId.Value, ct);

        if (result is null)
            return Unauthorized(new { message = "Нэвтрэх нэр эсвэл нууц үг буруу байна" });

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken,
            result.ExpiresAt,
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
    {
        var result = await _userService.RefreshAsync(request.RefreshToken, ct);

        if (result is null)
            return Unauthorized(new { message = "Refresh token хүчингүй эсвэл хугацаа дууссан" });

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken,
            result.ExpiresAt,
        });
    }
}

public record LoginRequest(string Email, string Password);
public record RefreshRequest(string RefreshToken);
