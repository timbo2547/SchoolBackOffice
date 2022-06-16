using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Domain.Entities;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Student> Students { get; }
        DbSet<GradeLevel> GradeLevels { get; }
        DbSet<StaffMember> StaffMembers { get; }
        DbSet<StaffUser> StaffUsers { get; }
    }
}