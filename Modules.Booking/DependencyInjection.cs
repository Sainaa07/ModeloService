using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Identity.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Modules.Booking
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBookingModule(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<BookingDbContext>(options =>
                options.UseNpgsql(
                    config.GetConnectionString("DefaultConnection"),
                    npgsql => npgsql.MigrationsHistoryTable(
                        "__EFMigrationsHistory",
                        "booking"
                    )
                )
            );
            return services;
        }
    }
}
