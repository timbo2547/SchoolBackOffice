using SchoolBackOffice.Domain.Common;

namespace SchoolBackOffice.Domain.Entities
{
    public class GradeLevel : BaseAuditableEntity
    {
        public int GradeLevelId { get; set; }
        public string GradeName { get; set; }
        public string GradeDescription { get; set; }
        public int GradeSort { get; set; }
    }
}