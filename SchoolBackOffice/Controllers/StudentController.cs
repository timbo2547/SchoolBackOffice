using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Domain.Entities;
using SchoolBackOffice.Models;

namespace SchoolBackOffice.Controllers
{
    public class StudentController : Controller
    {
        private readonly IGradeLevelService _gradeLevelService;
        private readonly IStudentUserService _studentUserService;
        
        public StudentController(IGradeLevelService gradeLevelService, IStudentUserService studentUserService)
        {
            _gradeLevelService = gradeLevelService;
            _studentUserService = studentUserService;
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var res = await _gradeLevelService
                .GetGradeLevelsAsync();
            
            var vm = new CreateStudentViewModel()
            {
                GradeLevels =  res.Select(x => new GradeLevelViewModel
                {
                    GradeName = x.GradeDisplayName,
                    GradeId = x.GradeLevelId,
                }).ToList()
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
            }

            return View(model);
        }
        
    }
}