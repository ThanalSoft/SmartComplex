using System.ComponentModel.DataAnnotations;

namespace ThanalSoft.SmartComplex.Web.Models.Account
{
    public class UserLoginModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}