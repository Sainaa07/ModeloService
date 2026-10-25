using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Identity.Infrastructure.Persistence;
using Modules.Identity.Application.Services;
namespace Modules.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration config) {
            services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(
                config.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    "identity"
                )
            )
        );
            services.AddScoped<UserService>();
            return services;
        }
    }
}
