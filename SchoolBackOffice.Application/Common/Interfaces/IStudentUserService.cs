using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolBackOffice.Domain.Entities;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IStudentUserService
    {
        Task<StudentUser> GetUserAsync(int studentUserId);
        Task<IEnumerable<StudentUser>> GetUsersAsync();
        Task<(string[] Error, int StudentUserId)> CreateStudentUserAsync(string email, string password, string firstName, string lastName, int gradeLevelId);
        Task<(string[] Error, int StudentUserId)> CreateStudentUserAsync(StudentUser studentUser);
        Task<int> UpdateStudentUserAsync(StudentUser studentUser);
    }
}