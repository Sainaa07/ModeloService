using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Domain.Entities;
using Modules.Identity.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace Modules.Identity.Application.Services;

internal sealed class UserService(
    UserManager<AppUser> userManager,
    TokenService tokenService,
    IdentityDbContext db)
{
    public async Task<LoginResult?> LoginAsync(
        string email, string password, Guid tenantId, CancellationToken ct)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.TenantId == tenantId, ct);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return null;

        return await IssueTokensAsync(user, ct);
    }

    public async Task<LoginResult?> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var hash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

        var stored = await db.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash, ct);

        if (stored is null || stored.RevokedAt is not null || stored.ExpiresAt < DateTime.UtcNow)
            return null;

        var user = await userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null) return null;

        stored.RevokedAt = DateTime.UtcNow;

        var result = await IssueTokensAsync(user, ct);

        stored.ReplacedById = result.RefreshTokenId;
        await db.SaveChangesAsync(ct);

        return result;
    }

    private async Task<LoginResult> IssueTokensAsync(AppUser user, CancellationToken ct)
    {
        var accessToken = tokenService.GenerateAccessToken(user);
        var (raw, hash) = tokenService.GenerateRefreshToken();

        var entity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = hash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        };

        db.RefreshTokens.Add(entity);
        await db.SaveChangesAsync(ct);

        var expiresAt = DateTime.UtcNow.AddMinutes(tokenService.AccessTokenExpiryMinutes);
        return new LoginResult(accessToken, raw, entity.Id, expiresAt);
    }
}

internal record LoginResult(
    string AccessToken,
    string RefreshToken,
    Guid RefreshTokenId,
    DateTime ExpiresAt);
