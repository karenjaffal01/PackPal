using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
        }

        public string CreateToken(User user)
        {
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");

            if (issuer == null || audience == null || secret == null)
                throw new Exception("JWT environment variables not configured properly.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken() => _authRepository.generateRefreshToken();

        public async Task SaveRefreshToken(int userId, string refreshToken)
        {
            await _authRepository.SaveRefreshToken(userId, refreshToken);
        }

        public async Task<RefreshTokenDto?> ValidateRefreshToken(RefreshTokenRequest request)
        {
            var stored = await _authRepository.GetRefreshToken(request.userId);
            if (stored == null || stored.IsRevoked || stored.ExpiresAt < DateTime.UtcNow)
                return null;

            var verification = _hasher.VerifyHashedPassword("refreshToken", stored.TokenHash, request.RefreshToken);
            if (verification != PasswordVerificationResult.Success)
                return null;

            var user = await _userRepository.GetUserById(request.userId);
            if (user == null)
                return null;

            var newRefreshToken = GenerateRefreshToken();
            await _authRepository.SaveRefreshToken(user.Id, newRefreshToken);

            return new RefreshTokenDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = newRefreshToken
            };
        }
    }
}
