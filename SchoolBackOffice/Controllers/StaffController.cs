using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["UserId"] = id;

            var u = _context.Users
                .SingleOrDefault(x => x.Id == id);

            if (u == null)
                return View();
            
            var roleList = await _identityService.GetRoleNames();
            var userRoles = await _identityService.GetUserRoles(u.Id);
            
            var vm = new EditStaffViewModel
            {
                StaffId = id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Roles = roleList.Select(x => new RoleViewModel
                {
                    Name = x, 
                    IsSelected = userRoles.Contains(x)
                }).ToList()
            };

            return View(vm);
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStaffViewModel model, string userId)
        {
            ViewData["UserId"] = userId;
            var u = _context.Users
                .SingleOrDefault(x => x.Id == userId);

            if (u == null)
            {
                _logger.LogWarning($"User with userId: '{userId} not round");
                ModelState.AddModelError("", "User not found");
                return View(model);
            }
            
            u.FirstName = model.FirstName;
            u.LastName = model.LastName;
            u.Email = model.Email;

            // if password is not null then update it
            if (!string.IsNullOrEmpty(model.Password))
            {
                var res = await _identityService.ResetPassword(userId, model.Password);
                if (!res.Succeeded)
                {
                    foreach (var error in res.Errors)
                        ModelState.AddModelError("", error);
                    return View(model);
                }
            }

            var selectedRoles = model.Roles
                .Where(x => x.IsSelected)
                .Select(x => x.Name)
                .ToArray();
            if (selectedRoles.Any())
                await _identityService.AddUserToRolesAsync(userId, selectedRoles);
            
            var unSelectedRoles = model.Roles               
                .Where(x => !x.IsSelected)
                .Select(x => x.Name)
                .ToArray();
            if (unSelectedRoles.Any())
                await _identityService.RemoveUserFromRolesAsync(userId, unSelectedRoles);
            
            await _context.SaveChangesAsync();
            _logger.LogWarning($"UserId: '{userId}', User: {u.LastName}, {u.FirstName} updated");
            return RedirectToAction("StaffRoster", "Dashboard", new { area = "" });
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var res = await _identityService.GetRoleNames();
            var vm = new CreateStaffViewModel
            {
                Roles = res.Select(x => new RoleViewModel {Name = x}).ToList()
            };
            
            return View(vm);
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
                    
                    var roles = model.Roles
                        .Where(x => x.IsSelected)
                        .Select(x => x.Name)
                        .ToArray();

                    if (roles.Any())
                    {
                        await _identityService.AddUserToRolesAsync(res.UserId, roles);
                        _logger.LogInformation($"Added '{model.LastName}, {model.FirstName}' to {roles} Role");
                    }
                    return Redirect("/Dashboard/StaffRoster");
                }
                
                foreach (var error in res.Result.Errors)
                    ModelState.AddModelError("", error);
            }

            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            ViewData["UserId"] = id;
            var u = await _context.Users
                .SingleOrDefaultAsync(x => x.Id == id);

            if (u == null)
                return Error();

            var res = await _identityService.DeleteUserAsync(id);
            
            return res.Succeeded ? Redirect("/Dashboard/StaffRoster") : Error();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}