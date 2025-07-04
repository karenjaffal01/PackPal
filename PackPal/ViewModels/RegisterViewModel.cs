using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PackPal.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(40,MinimumLength = 8, ErrorMessage = "Password must be of length between 8 and 40")]
        [Compare("ConfirmPassword",ErrorMessage = "Password and Confirmation does not match")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Confirmation is required")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]

        public string ConfirmPassword { get; set; }

    }
}
