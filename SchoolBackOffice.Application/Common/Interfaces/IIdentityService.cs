using System.Threading.Tasks;
using SchoolBackOffice.Application.Common.Models;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> ResetPassword(string userId, string newPassword);
        Task<string> GetUserNameAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> AuthorizeAsync(string userId, string policyName);
        Task<string[]> GetRoleNames();
        Task<string[]> GetUserRoles(string userId);
        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string firstName, string lastName, bool isStaff);
        Task<Result> AddUserToRolesAsync(string userId, string[] roles);
        Task<Result> RemoveUserFromRolesAsync(string userId, string[] roles);
        Task<Result> DeleteUserAsync(string userId);
        Task<string> GetEmailConfirmationTokenAsync(string aspUserId);
    }
}