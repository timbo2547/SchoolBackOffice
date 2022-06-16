using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Models;

namespace SchoolBackOffice.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IStaffUserService _staffUserService;

        public DashboardController(ILogger<DashboardController> logger, IStaffUserService staffUserService)
        {
            _logger = logger;
            _staffUserService = staffUserService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }        
        
        public IActionResult Calendar()
        {
            return View();
        }
        
        public async Task<IActionResult> StaffRoster()
        {
            var s = await _staffUserService
                .GetStaffUsersAsync();
            
            return View(s);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}