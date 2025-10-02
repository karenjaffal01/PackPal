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
using TravelApp.Domain.Dto;
using TravelApp.Domain.Entities;
using TravelApp.Domain.Requests;
using TravelApp.Persistence.Interfaces;

namespace TravelApp.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();
        private readonly IAuthRepository authRepository;
        private readonly IUserRepository userRepository;
        public AuthService(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }
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
        public async Task<RefreshTokenDto> ValidateRefreshToken(RefreshTokenRequest request)
        {
            var tokenEntity = await authRepository.GetRefreshToken(request.userId);
            if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiresAt < DateTime.UtcNow)
                return null;
            var result = _hasher.VerifyHashedPassword("refreshToken", tokenEntity.TokenHash, request.RefreshToken);
            if (result != PasswordVerificationResult.Success)
                return null;

            var user = await userRepository.GetUserById(request.userId);
            var refresh = authRepository.generateRefreshToken();
            await authRepository.SaveRefreshToken(request.userId,refresh);
            var response = new RefreshTokenDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = refresh
            };
            return response;
        }
        
    }
}
