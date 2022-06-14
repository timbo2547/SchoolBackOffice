using SchoolBackOffice.Domain.Common;

namespace SchoolBackOffice.Domain.Entities
{
    public class StaffMember : BaseAuditableEntity
    {
        public int StaffMemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}