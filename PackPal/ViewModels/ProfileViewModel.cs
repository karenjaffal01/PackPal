using PackPal.Models;
using System.ComponentModel.DataAnnotations;

namespace PackPal.ViewModels
{
    public class ProfileViewModel
    {
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string CustomUsername { get; set; }

        public IFormFile Photo { get; set; }

        public string PhotoUrl { get; set; } //for display
        public List<Trip> Trips { get; set; } = new List<Trip>();
    }
}
