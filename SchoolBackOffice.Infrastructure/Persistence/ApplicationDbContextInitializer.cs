using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolBackOffice.Infrastructure.Identity;

namespace SchoolBackOffice.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationDbContextInitializer> _logger;

        public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
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