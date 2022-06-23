using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Domain.Entities;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<GradeLevel> GradeLevels { get; }
        DbSet<StaffUser> StaffUsers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}