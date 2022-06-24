using System.Linq;
using System.Threading.Tasks;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Interfaces;
using SchoolBackOffice.Models;
using SchoolBackOffice.ViewModels;

namespace SchoolBackOffice.Services
{
    public class StudentUserViewModelService : IStudentUserViewModelService
    {

        private readonly IStudentUserService _studentUserService;
        private readonly IGradeLevelService _gradeLevelService;
        
        public StudentUserViewModelService(IStudentUserService studentUserService, IGradeLevelService gradeLevelService)
        {
            _studentUserService = studentUserService;
            _gradeLevelService = gradeLevelService;
        }
        
        public async Task<(EditStudentViewModel EditStudentViewModel, string Error)> GetEditStudentViewModel(int studentUserId)
        {
            var studentUser = await _studentUserService.GetUserAsync(studentUserId);
            var vm = new EditStudentViewModel();
            
            if (studentUser == null)
                return (vm, "User Not Found");

            vm.StudentId = studentUserId;
            vm.Email = studentUser.Email;
            vm.FirstName = studentUser.FirstName;
            vm.LastName = studentUser.LastName;
            vm.GradeLevelId = studentUser.GradeLevelId;

            var res = await _gradeLevelService
                .GetGradeLevelsAsync();

            vm.GradeLevels = res.Select(x => new GradeLevelViewModel()
            {
                GradeId = x.GradeLevelId,
                GradeName = x.GradeDisplayName,
            }).ToList();
            return (vm, "");
        }
    }
}