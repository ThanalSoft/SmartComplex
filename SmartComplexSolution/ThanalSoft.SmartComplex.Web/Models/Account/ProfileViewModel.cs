using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        [Display(Name = "Blood Group")]
        public int? BloodGroupId { get; set; }

        public SelectListItem[] BloodGroups { get; set; }
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
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Password mismatch.")]
        public string ConfirmNewPassword { get; set; }
        
    }
}