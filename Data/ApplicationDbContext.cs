using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiTenantTaskManager.Models;

namespace MultiTenantTaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(u => u.TenantId);

            builder.Entity<TaskItem>()
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(t => t.TenantId);

            builder.Entity<TaskItem>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId);
        }
    }
}
