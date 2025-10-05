using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Entities;
using TravelApp.Persistence.Data;
using TravelApp.Persistence.Interfaces;

namespace TravelApp.Persistence.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly TravelDbContext _context;
        public UserRepository(TravelDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            return user;
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

        public Task<bool> UserExistsAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
