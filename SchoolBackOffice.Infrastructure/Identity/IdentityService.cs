using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Application.Common.Models;
using SchoolBackOffice.Infrastructure.Persistence;

namespace SchoolBackOffice.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _context = context;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            return user.UserName;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);
            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string firstName, string lastName, bool isStaff)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                FirstName = firstName,
                LastName = lastName,
                IsStaff = isStaff,
            };

            var result = await _userManager.CreateAsync(user, password);
            return (result.ToApplicationResult(), user.Id);
        }
        
        public async Task<Result> ResetPassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.ToApplicationResult();
        }

        public async Task<Result> AddUserToRolesAsync(string userId, string[] roles)
        {
            var userRoles = await GetUserRoles(userId);
            var newRoles = roles
                .Where(x => !userRoles.Contains(x))
                .ToArray();
            
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var result = await _userManager.AddToRolesAsync(user, newRoles);
            return result.ToApplicationResult();
        }

        public async Task<Result> RemoveUserFromRolesAsync(string userId, string[] roles)
        {
            var userRoles = await GetUserRoles(userId);
            var removeRoles = roles
                .Where(x => userRoles.Contains(x))
                .ToArray();
            
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var result = await _userManager.RemoveFromRolesAsync(user, removeRoles);
            return result.ToApplicationResult();
        }

        public async Task<string[]> GetRoleNames()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles.Select(x => x.Name).ToArray();
        }

        public async Task<string[]> GetUserRoles(string userId)
        {
            var userRoles = await _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();

            var roles = await _context.Roles
                .Where(x => userRoles.Contains(x.Id))
                .Select(x => x.Name)
                .ToArrayAsync();

            return roles;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            var result = await _authorizationService.AuthorizeAsync(principal, policyName);
            return result.Succeeded;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.ToApplicationResult();
        }
        
        public async Task<string> GetEmailConfirmationTokenAsync(string aspUserId)
        {
            var applicationUser = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == aspUserId);
            
            return await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
        }
        
    }
}