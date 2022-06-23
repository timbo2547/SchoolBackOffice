using System.Collections.Generic;

namespace SchoolBackOffice.Application.Common.Models
{
    public class EditStaffViewModelDto
    {
        public EditStaffViewModelDto()
        {
            Roles = new List<RoleViewModelDto>();
        }
        
        public int StaffUserId { get; set; }
        public string AspUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public List<RoleViewModelDto> Roles { get; set; }
    }
}