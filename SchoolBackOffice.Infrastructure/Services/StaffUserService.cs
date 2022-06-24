using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Infrastructure.Identity;
using SchoolBackOffice.Infrastructure.Persistence;

namespace SchoolBackOffice.Infrastructure.Services
{
    public class StaffUserService : IStaffUserService
    {
        private readonly IIdentityService _identityService;
        private readonly ApplicationDbContext _context;

        public StaffUserService(IIdentityService identityService, ApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task<StaffUser> GetStaffUserAsync(int staffUserId)
        {
            return await _context.StaffUsers
                .FirstOrDefaultAsync(x => x.StaffUserId == staffUserId);
        }

        public async Task<IEnumerable<StaffUser>> GetStaffUsersAsync()
        {
            return await _context.StaffUsers
                .ToListAsync();
        }

        public async Task<(string[] Error, int StaffUserId)> CreateStaffUserAsync(string email, string password, string firstName, string lastName, string[] roles)
        {
            var res = await _identityService.CreateUserAsync(email, password);
            if (res.Result.Succeeded)
            {
                if (roles.Any()) 
                    await _identityService.AddUserToRolesAsync(res.UserId, roles);
                
                var staffUser = new StaffUser
                {
                    AspUserId = res.UserId,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                };
                
                return await CreateStaffUserAsync(staffUser);
            }

            return (res.Result.Errors, 0);
        }

        public async Task<(string[] Error, int StaffUserId)> CreateStaffUserAsync(StaffUser staffUser)
        {
            _context.StaffUsers.Add(staffUser);
            var r = await _context.SaveChangesAsync();
            return (Array.Empty<string>(), staffUser.StaffUserId);
        }

        public async Task<int> UpdateStaffUserAsync(StaffUser staffUser)
        {
            _context.StaffUsers.Update(staffUser);
            return await _context.SaveChangesAsync();
        }
    }
}