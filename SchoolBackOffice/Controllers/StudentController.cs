using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Interfaces;
using SchoolBackOffice.Models;
using SchoolBackOffice.ViewModels;

namespace SchoolBackOffice.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IGradeLevelService _gradeLevelService;
        private readonly IStudentUserService _studentUserService;
        private readonly IIdentityService _identityService;
        private readonly IEmailSender _emailSender;
        private readonly IStudentUserViewModelService _studentUserViewModelService;
        
        public StudentController(IGradeLevelService gradeLevelService, IStudentUserViewModelService studentUserViewModelService, IStudentUserService studentUserService, IIdentityService identityService, IEmailSender emailSender, ILogger<StudentController> logger)
        {
            _logger = logger;
            _gradeLevelService = gradeLevelService;
            _studentUserService = studentUserService;
            _identityService = identityService;
            _emailSender = emailSender;
            _studentUserViewModelService = studentUserViewModelService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var vm = new CreateStudentViewModel()
            {
                GradeLevels = await GetGradeViewModel(),
            };
            
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var newStudentUser = await _studentUserService.CreateStudentUserAsync(
                    model.Email, model.Password, model.FirstName, model.LastName, model.GradeLevelId);

                if (newStudentUser.Error.Any())
                {
                    foreach (var error in newStudentUser.Error)
                        ModelState.AddModelError("", error);
                    model.GradeLevels = await GetGradeViewModel();
                    return View(model);
                }
                
                var studentUser = await _studentUserService.GetUserAsync(newStudentUser.StudentUserId);
                var token = await _identityService.GetEmailConfirmationTokenAsync(studentUser.AspUserId);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                await SendEmail(model, returnUrl, studentUser, token);
                
                return RedirectToAction("StudentRoster", "Dashboard");
            }

            model.GradeLevels = await GetGradeViewModel();
            return View(model);
        }
        
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["StudentUserId"] = id;

            var (dto, error) = await _studentUserViewModelService
                .GetEditStudentViewModel(id);
            
            if (error.Any())
                ModelState.AddModelError("", error);

            return View(dto);
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStudentViewModel model, int studentUserId)
        {
            ViewData["StudentUserId"] = studentUserId;
            var studentUser = await _studentUserService.GetUserAsync(studentUserId);
            
            if (studentUser == null)
            {
                _logger.LogWarning($"User with userId: '{studentUserId} not round");
                ModelState.AddModelError("", "User not found");
                return View(model);
            }
            
            studentUser.FirstName = model.FirstName;
            studentUser.LastName = model.LastName;
            studentUser.Email = model.Email;
            studentUser.GradeLevelId = model.GradeLevelId;
            
            // if password is not null then update it
            if (!string.IsNullOrEmpty(model.Password))
            {
                var res = await _identityService.ResetPassword(studentUser.AspUserId, model.Password);
                if (!res.Succeeded)
                {
                    foreach (var error in res.Errors)
                        ModelState.AddModelError("", error);
                    return View(model);
                }
            }
            
            await _studentUserService.UpdateStudentUserAsync(studentUser);
            
            _logger.LogInformation($"StudentUserId: '{studentUserId}', User: {studentUser.LastName}, {studentUser.FirstName} updated");
            return RedirectToAction("StudentRoster", "Dashboard", new { area = "" });
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["StudentUserId"] = id;
            var u = await _studentUserService.GetUserAsync(id);
            
            if (u == null)
                return Error();

            var res = await _identityService.DeleteUserAsync(u.AspUserId);
            return res.Succeeded ? RedirectToAction("StudentRoster", "Dashboard") : Error();
        }
        
        private async Task SendEmail(CreateStudentViewModel model, string returnUrl, StudentUser studentUser, string token)
        {
            var emailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new {area = "Identity", userId = studentUser.AspUserId, code = token, returnUrl = returnUrl},
                protocol: Request.Scheme);

            var message =
                "New Account Created: " + Environment.NewLine +
                $"User Name: {model.Email}" + Environment.NewLine +
                $"Initial Password: {model.Password}" + Environment.NewLine +
                $"Confirm your account with this link: {emailConfirmationUrl}" + Environment.NewLine +
                $"Please change your password at your earliest convenience.";

            await _emailSender.SendEmailAsync(model.Email, "Account Created", message);
        }
        
        private async Task<List<GradeLevelViewModel>> GetGradeViewModel()
        {
            var res = await _gradeLevelService
                .GetGradeLevelsAsync();

            return res.Select(x => new GradeLevelViewModel
            {
                GradeName = x.GradeDisplayName,
                GradeId = x.GradeLevelId,
            }).ToList();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}