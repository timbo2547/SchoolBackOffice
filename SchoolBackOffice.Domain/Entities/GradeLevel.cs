using SchoolBackOffice.Domain.Common;

namespace SchoolBackOffice.Domain.Entities
{
    public class GradeLevel : BaseAuditableEntity
    {
        public GradeLevel()
        {
            
        }
        public GradeLevel(string gradeName, string gradeDescription, int gradeSort)
        {
            GradeName = gradeName;
            GradeDescription = gradeDescription;
            GradeSort = gradeSort;
        }
        
        public int GradeLevelId { get; set; }
        public string GradeName { get; set; }
        public string GradeDescription { get; set; }
        public int GradeSort { get; set; }
        public string GradeDisplayName => $"({GradeName}) - {GradeDescription}";
    }
}