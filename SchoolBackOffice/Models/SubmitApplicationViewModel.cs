using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolBackOffice.Models
{
    public class SubmitApplicationViewModel
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}