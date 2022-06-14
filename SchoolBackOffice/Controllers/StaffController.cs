using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Infrastructure.Persistence;
using SchoolBackOffice.Models;

namespace SchoolBackOffice.Controllers
{
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        
        public StaffController(ILogger<StaffController> logger, ApplicationDbContext context, IIdentityService identityService)
        {
            _logger = logger;
            _context = context;
            _identityService = identityService;
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
                var res = await _identityService.CreateUserAsync(model.Email, model.Password, model.FirstName, model.LastName, true);
                if (res.Result.Succeeded)
                {
                    _logger.LogInformation($"New Staff User '{model.LastName}, {model.FirstName}' created");
                    await _identityService.AddUserToRolesAsync(res.UserId, new[] {"Staff"});
                    _logger.LogInformation($"Added '{model.LastName}, {model.FirstName}' to 'Staff' Role");
                    return Redirect("/Dashboard/StaffRoster");
                }
                
                foreach (var error in res.Result.Errors)
                    ModelState.AddModelError("", error);
            }

            return View(model);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}