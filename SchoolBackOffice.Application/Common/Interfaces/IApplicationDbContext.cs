using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Domain.Entities;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<GradeLevel> GradeLevels { get; }
        DbSet<StaffUser> StaffUsers { get; }
    }
}