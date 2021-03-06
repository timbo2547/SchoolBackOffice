using SchoolBackOffice.Domain.Common;

namespace SchoolBackOffice.Domain.Entities
{
    public class StudentUser : BaseAuditableEntity
    {
        public int StudentUserId { get; set; }
        public string AspUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int? GradeLevelId { get; set; } = null;
        public GradeLevel Grade { get; set; }
        public int EnrollmentStatusId { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public string DisplayName => $"{LastName}, {FirstName}";

    }
}