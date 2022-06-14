using System;
using SchoolBackOffice.Domain.Common;

namespace SchoolBackOffice.Domain.Entities
{
    public class Student : BaseAuditableEntity
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GradeLevel CurrentGrade { get; set; }
        public DateTime EnrollDate { get; set; }
    }
}