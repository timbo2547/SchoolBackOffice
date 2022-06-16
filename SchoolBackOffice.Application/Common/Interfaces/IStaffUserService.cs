using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolBackOffice.Application.Common.Models;
using SchoolBackOffice.Domain.Entities;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IStaffUserService
    {
        Task<StaffUser> GetStaffUserAsync(int staffUserId);
        Task<IEnumerable<StaffUser>> GetStaffUsersAsync();
        Task<(string[] Error, int StaffUserId)> CreateStaffUserAsync(string email, string password, string firstName, string lastName, string[] roles);
        Task<(string[] Error, int StaffUserId)> CreateStaffUserAsync(StaffUser staffUser);
        Task<int> UpdateStaffUserAsync(StaffUser staffUser);
    }
}