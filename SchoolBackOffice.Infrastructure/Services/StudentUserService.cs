using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Infrastructure.Identity;
using SchoolBackOffice.Infrastructure.Persistence;

namespace SchoolBackOffice.Infrastructure.Services
{
    public class StudentUserService : IStudentUserService
    {
        private readonly IIdentityService _identityService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public StudentUserService(IIdentityService identityService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _identityService = identityService;
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<StudentUser> GetUserAsync(int studentUserId)
        {
            return await _context.StudentUsers
                .FirstOrDefaultAsync(x => x.StudentUserId == studentUserId);
        }

        public async Task<IEnumerable<StudentUser>> GetUsersAsync()
        {
            return await _context.StudentUsers
                .ToListAsync();
        }

        public async Task<(string[] Error, int StudentUserId)> CreateStudentUserAsync(string email, string password, string firstName, string lastName, int gradeLevelId)
        {
            var res = await _identityService.CreateUserAsync(email, password);
            if (res.Result.Succeeded)
            {
                var studentUser = new StudentUser()
                {
                    AspUserId = res.UserId,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Grade = new GradeLevel()
                };
                
                return await CreateStudentUserAsync(studentUser);
            }
            
            return (res.Result.Errors, 0);
        }

        public async Task<(string[] Error, int StudentUserId)> CreateStudentUserAsync(StudentUser studentUser)
        {
            _context.StudentUsers.Add(studentUser);
            var r = await _context.SaveChangesAsync();
            return (Array.Empty<string>(), studentUser.StudentUserId);
        }
    }
}