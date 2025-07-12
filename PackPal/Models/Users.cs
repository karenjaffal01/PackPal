using Microsoft.AspNetCore.Identity;

namespace PackPal.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
        public string CustomUsername { get; set; } 

        public string PhotoPath { get; set; }
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
