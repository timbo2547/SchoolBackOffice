using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolBackOffice.Models
{
    public class CreateStudentViewModel
    {
        public string StudentId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public int GradeLevelId { get; set; }
        [Display(Name = "Grade Level")]
        public List<GradeLevelViewModel> GradeLevels { get; set; } = new ();
    }
    
    public class GradeLevelViewModel
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
    }
}