using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Entities;
using TravelApp.Persistence.Data;
using TravelApp.Persistence.Interfaces;

namespace TravelApp.Persistence.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly TravelDbContext _context;
        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();
        public AuthRepository(TravelDbContext context)
        {
            _context = context;
        }
        public string generateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var random = Convert.ToBase64String(randomNumber);
            random = _hasher.HashPassword(random, random);
            return random;
        }
        public async Task SaveRefreshToken(int userId,string refreshToken)
        {
            var refresh = new RefreshToken
            {
                TokenHash = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
                UserId = userId
            };
            var user = _context.RefreshTokens.Add(refresh);
            await _context.SaveChangesAsync();
        }
        public async Task<RefreshToken> GetRefreshToken(int userId)
        {
            var user = await _context.Users
            .Include(u => u.RefreshTokens) 
            .FirstOrDefaultAsync(u => u.Id == userId);

            var latestToken = user?.RefreshTokens
                .OrderByDescending(rt => rt.CreatedAt)
                .FirstOrDefault();
            return latestToken;
        }
    }
}
