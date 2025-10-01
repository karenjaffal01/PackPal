using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Entities;
using TravelApp.Persistence.Data;

namespace TravelApp.Persistence.Repositories
{
    public class AuthRepository
    {
        private readonly TravelDbContext _context;
        public AuthRepository(TravelDbContext context)
        {
            _context = context;
        }
        public async Task SaveRefreshToken(string refreshToken)
        {
            var refresh = new RefreshToken
            {
                TokenHash = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            var user = _context.RefreshTokens.Add(refresh);
            await _context.SaveChangesAsync();
        }
    }
}
