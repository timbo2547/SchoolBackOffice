using System.Linq;
using System.Threading.Tasks;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Application.Common.Models;

namespace SchoolBackOffice.Services.StaffUsers
{
    public class StaffUserViewModelService : IStaffUserViewModelService
    {
        private readonly IIdentityService _identityService;
        private readonly IStaffUserService _staffUserService;

        public StaffUserViewModelService(IIdentityService identityService, IStaffUserService staffUserService)
        {
            _identityService = identityService;
            _staffUserService = staffUserService;
        }
        
        public async Task<(EditStaffViewModelDto EditStaffDto, string Error)> GetEditStaffViewModel(int staffUserId)
        {
            var staffUser = await _staffUserService.GetStaffUserAsync(staffUserId);
            var vm = new EditStaffViewModelDto();
            
            if (staffUser == null)
                return (vm, "User Not Found");
            
            var roleList = await _identityService.GetRoleNames();
            var userRoles = await _identityService.GetUserRoles(staffUser.AspUserId);

            vm.StaffUserId = staffUserId;
            vm.Email = staffUser.Email;
            vm.FirstName = staffUser.FirstName;
            vm.LastName = staffUser.LastName;
            vm.Roles = roleList.Select(x => new RoleViewModelDto()
            {
                Name = x,
                IsSelected = userRoles.Contains(x)
            }).ToList();

            return (vm, "");
        }
    }
}