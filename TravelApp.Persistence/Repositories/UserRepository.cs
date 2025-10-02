using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Entities;
using TravelApp.Persistence.Data;

namespace TravelApp.Persistence.Repositories
{
    public class UserRepository
    {
        private readonly TravelDbContext _context;
        public UserRepository(TravelDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

    }
}
