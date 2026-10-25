using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Domain.Entities;
using Modules.Identity.Infrastructure.Persistence;

namespace Modules.Identity;

internal class DevDataSeeder(IdentityDbContext db, UserManager<AppUser> userManager)
{
    public async Task SeedAsync()
    {
        var hasTenants = await db.Tenants.AnyAsync();
        var hasUsers = await userManager.Users.AnyAsync();

        if (hasTenants && hasUsers) return;

        Tenant salon1;

        if (!hasTenants)
        {
            salon1 = new Tenant { Name = "Salon 1", Slug = "salon1" };
            var salon2 = new Tenant { Name = "Salon 2", Slug = "salon2" };

            db.Tenants.AddRange(salon1, salon2);
            db.TenantSettings.AddRange(
                new TenantSettings { TenantId = salon1.Id },
                new TenantSettings { TenantId = salon2.Id }
            );
            await db.SaveChangesAsync();
        }
        else
        {
            salon1 = await db.Tenants.FirstAsync(t => t.Slug == "salon1");
        }

        if (!hasUsers)
        {
            var admin = new AppUser
            {
                UserName = "admin@salon1",
                Email = "admin@salon1.com",
                FullName = "Admin User",
                TenantId = salon1.Id,
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(admin, "Admin123!");
        }
    }
}
