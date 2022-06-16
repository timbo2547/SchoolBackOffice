using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Models;

namespace SchoolBackOffice.Controllers
{
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IIdentityService _identityService;
        private readonly IStaffUserService _staffUserService;

        public StaffController(ILogger<StaffController> logger, IIdentityService identityService, IStaffUserService staffUserService)
        {
            _logger = logger;
            _identityService = identityService;
            _staffUserService = staffUserService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["StaffUserId"] = id;

            var staffUser = await _staffUserService.GetStaffUserAsync(id);

            if (staffUser == null)
                return View();
            
            var roleList = await _identityService.GetRoleNames();
            var userRoles = await _identityService.GetUserRoles(staffUser.AspUserId);
            
            var vm = new EditStaffViewModel
            {
                StaffId = id,
                Email = staffUser.Email,
                FirstName = staffUser.FirstName,
                LastName = staffUser.LastName,
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
        public async Task<IActionResult> Edit(EditStaffViewModel model, int staffUserId)
        {
            ViewData["StaffUserId"] = staffUserId;
            
            var staffUser = await _staffUserService.GetStaffUserAsync(staffUserId);

            if (staffUser == null)
            {
                _logger.LogWarning($"User with userId: '{staffUserId} not round");
                ModelState.AddModelError("", "User not found");
                return View(model);
            }
            
            staffUser.FirstName = model.FirstName;
            staffUser.LastName = model.LastName;
            staffUser.Email = model.Email;

            // if password is not null then update it
            if (!string.IsNullOrEmpty(model.Password))
            {
                var res = await _identityService.ResetPassword(staffUser.AspUserId, model.Password);
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
                await _identityService.AddUserToRolesAsync(staffUser.AspUserId, selectedRoles);
            
            var unSelectedRoles = model.Roles               
                .Where(x => !x.IsSelected)
                .Select(x => x.Name)
                .ToArray();
            if (unSelectedRoles.Any())
                await _identityService.RemoveUserFromRolesAsync(staffUser.AspUserId, unSelectedRoles);

            await _staffUserService.UpdateStaffUserAsync(staffUser);
            
            _logger.LogInformation($"StaffUserId: '{staffUserId}', User: {staffUser.LastName}, {staffUser.FirstName} updated");
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
                var roles = model.Roles
                    .Where(x => x.IsSelected)
                    .Select(x => x.Name)
                    .ToArray();

                var newStaffUser = await _staffUserService.CreateStaffUserAsync(
                    model.Email, model.Password, model.FirstName, model.LastName, roles);
                
                _logger.LogInformation($"New Staff User '{model.LastName}, {model.FirstName}' created");

                if (!newStaffUser.Error.Any()) 
                    return RedirectToAction("StaffRoster", "Dashboard");
                
                _logger.LogWarning($"Error Creating User '{model.LastName}, {model.FirstName}'");
                foreach (var error in newStaffUser.Error)
                    ModelState.AddModelError("", error);
            }

            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["StaffUserId"] = id;
            var u = await _staffUserService.GetStaffUserAsync(id);
            
            if (u == null)
                return Error();

            var res = await _identityService.DeleteUserAsync(u.AspUserId);
            return res.Succeeded ? RedirectToAction("StaffRoster", "Dashboard") : Error();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}