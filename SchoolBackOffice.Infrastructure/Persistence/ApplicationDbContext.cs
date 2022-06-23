using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Infrastructure.Identity;
using SchoolBackOffice.Infrastructure.Persistence.Interceptors;

namespace SchoolBackOffice.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
            : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(b =>
            {
                b
                    .HasOne<StaffUser>()
                    .WithOne()
                    .HasForeignKey("StaffUser", "AspUserId");                b
                    .HasOne<StudentUser>()
                    .WithOne()
                    .HasForeignKey("StudentUser", "AspUserId");
            });
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public DbSet<GradeLevel> GradeLevels => Set<GradeLevel>();
        public DbSet<StaffUser> StaffUsers => Set<StaffUser>();
        public DbSet<StudentUser> StudentUsers => Set<StudentUser>();
    }
}