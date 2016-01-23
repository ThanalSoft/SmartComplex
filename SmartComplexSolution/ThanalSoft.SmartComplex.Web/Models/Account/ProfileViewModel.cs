using System.ComponentModel.DataAnnotations;

namespace ThanalSoft.SmartComplex.Web.Models.Account
{
    public class ProfileViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }

    }
}