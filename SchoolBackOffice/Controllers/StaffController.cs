using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Infrastructure.Persistence;
using SchoolBackOffice.Models;

namespace SchoolBackOffice.Controllers
{
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly ApplicationDbContext _context;
        
        public StaffController(ILogger<StaffController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStaffViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var newStaff = new StaffMember()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                _context.StaffMembers.Add(newStaff);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"New Staff Member '{newStaff.LastName}, {newStaff.FirstName}' Added");
                return Redirect("/Dashboard/StaffRoster");
            }

            return View(model);
        }
    }
}