using SchoolBackOffice.Domain.Common;

namespace SchoolBackOffice.Domain.Entities
{
    public class EnrollmentStatus : BaseAuditableEntity
    {
        public EnrollmentStatus()
        {
            
        }
        public EnrollmentStatus(string statusName)
        {
            StatusName = statusName;
        }
        public int EnrollmentStatusId { get; set; }
        public string StatusName { get; set; }
    }
}