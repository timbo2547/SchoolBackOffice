using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Infrastructure.Identity;

namespace SchoolBackOffice.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly IDateTime _dateTime;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IDateTime dateTime)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _dateTime = dateTime;
        }
        
        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlite())
                {
                    await _context.Database.MigrateAsync();

                    if (await _context.Database.EnsureCreatedAsync())
                    {
                        
                    }
                    
                    _logger.LogInformation("Migration Complete");
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
        
        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static IEnumerable<GradeLevel> GetDefaultGrades()
        {
            return new List<GradeLevel>()
            {
                new ("K", "Kindergarten", 1),                
                new ("1", "First Grade", 2),                
                new ("2", "Second Grade", 3),                
                new ("3", "Third Grade", 4),                
                new ("4", "Fourth Grade", 5),
                new ("5", "Fifth Grade", 6),
                new ("6", "Sixth Grade", 7),
                new ("7", "Seventh Grade", 8),
                new ("8", "Eighth Grade", 9),
                new ("9", "Freshman", 10),
                new ("10", "Sophomore", 11),
                new ("11", "Junior", 12),
                new ("12", "Senior", 13),
            };
        }

        private async Task TrySeedAsync()
        {
            // Default roles
            var administratorRole = new IdentityRole("Administrator");
            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await _roleManager.CreateAsync(administratorRole);
                _logger.LogInformation($"Created {administratorRole.Name} Role");
            }
            
            var staffRole = new IdentityRole("Staff");
            if (_roleManager.Roles.All(r => r.Name != staffRole.Name))
            {
                await _roleManager.CreateAsync(staffRole);
                _logger.LogInformation($"Created {staffRole.Name} Role");
            }
            
            // Default Grades
            if (!await _context.GradeLevels.AnyAsync())
            {
                await _context.GradeLevels
                    .AddRangeAsync(GetDefaultGrades());

                await _context.SaveChangesAsync();
            }
            
            // Default users
            var administrator = new ApplicationUser
            {
                UserName = "administrator@localhost", 
                Email = "administrator@localhost", 
                EmailConfirmed = true,
                IsStaff = true,
            };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Administrator1!");
                _logger.LogInformation($"Created {administrator.UserName} user");
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
                _logger.LogInformation($"Added {administrator.UserName} to {administratorRole.Name} Role");
            }
        }
    }
}