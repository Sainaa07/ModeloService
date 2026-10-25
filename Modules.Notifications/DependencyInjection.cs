using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Identity.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Booking
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationModule(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<NotificationsDbContext>(options =>
               options.UseNpgsql(
                   config.GetConnectionString("DefaultConnection"),
                   npgsql => npgsql.MigrationsHistoryTable(
                       "__EFMigrationsHistory",
                       "notifications"
                   )
               )
           );
           return services;
        }
    }
}
