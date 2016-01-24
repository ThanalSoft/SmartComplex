using System.ComponentModel.DataAnnotations;

namespace ThanalSoft.SmartComplex.Web.Models.Account
{
    public class ProfileUpdateViewModel : BaseViewModel
    {
        public ProfileViewModel ProfileViewModel { get; set; }

        public CredentialViewModel CredentialViewModel { get; set; }

        public string Email { get; set; }
        
    }

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
    }

    public class CredentialViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Old Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Password mismatch.")]
        public string ConfirmNewPassword { get; set; }
        
    }
}