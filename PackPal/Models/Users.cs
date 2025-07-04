using Microsoft.AspNetCore.Identity;

namespace PackPal.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }

    }
}
