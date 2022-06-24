using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolBackOffice.Domain.Entities;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IGradeLevelService
    {
        Task<GradeLevel> GetGradeLevelAsync(int gradeLevelId);
        Task<IEnumerable<GradeLevel>> GetGradeLevelsAsync();
        // Task<(string[] Error, int StudentUserId)> CreateStudentUserAsync(string email, string password, string firstName, string lastName, int gradeLevelId);
        // Task<(string[] Error, int StudentUserId)> CreateStudentUserAsync(StudentUser staffUser);
    }
}