using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PackPal.Models;

namespace PackPal.Data
{
    public class AppDb : IdentityDbContext<Users>
    {
        public AppDb(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Trip> Trips { get; set; }
    }
}
