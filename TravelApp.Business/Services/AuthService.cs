using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Business.Interfaces;
using TravelApp.Domain.Entities;

namespace TravelApp.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();
        public string CreateToken(User user)
        {
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if(issuer == null  || audience== null || secret == null)
            {
                throw new Exception("JWT settings are not configured properly in environment variables.");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),                
                new Claim(ClaimTypes.Email, user.Email)                   
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)); 

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience:audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public RefreshToken generateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var random =  Convert.ToBase64String(randomNumber);
            random = _hasher.HashPassword(random, random);
            var refresh = new RefreshToken
            {
                TokenHash = random,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            return refresh;
        }
    }
}
