using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PackPal.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password must be of length between 8 and 40")]
        [Compare("ConfirmPassword", ErrorMessage = "Password and Confirmation does not match")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password Confirmation is required")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
