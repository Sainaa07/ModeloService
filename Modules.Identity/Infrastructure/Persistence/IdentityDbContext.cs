using Microsoft.EntityFrameworkCore;
using Modules.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Identity.Infrastructure.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("identity");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(x => x.Email)
                    .HasMaxLength(256);

                entity.HasIndex(x => x.Email)
                    .IsUnique();

                entity.Property(x => x.FullName)
                    .HasMaxLength(200);
            });
        }
    }
}
