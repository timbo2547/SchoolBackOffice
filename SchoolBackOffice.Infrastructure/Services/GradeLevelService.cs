using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Infrastructure.Persistence;

namespace SchoolBackOffice.Infrastructure.Services
{
    public class GradeLevelService : IGradeLevelService
    {
        private readonly ApplicationDbContext _context;

        public GradeLevelService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<GradeLevel> GetGradeLevelAsync(int gradeLevelId)
        {
            return await _context.GradeLevels
                .FirstOrDefaultAsync(x => x.GradeLevelId == gradeLevelId);
        }

        public async Task<IEnumerable<GradeLevel>> GetGradeLevelsAsync()
        {
            return await _context.GradeLevels
                .ToListAsync();
        }
    }
}