using System.ComponentModel.DataAnnotations;

namespace PROG7311_PART_2.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; } // Dropdown or radio input on frontend


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

}
