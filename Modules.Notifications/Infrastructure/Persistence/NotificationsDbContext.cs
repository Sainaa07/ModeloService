using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Identity.Infrastructure.Persistence
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("notifications");
        }
    }
}
