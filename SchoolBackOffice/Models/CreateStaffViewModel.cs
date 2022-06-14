using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolBackOffice.Models
{
    public class CreateStaffViewModel
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}