using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Business.Interfaces;
using TravelApp.Domain.Common;
using TravelApp.Domain.Entities;
using TravelApp.Domain.Requests;
using TravelApp.Persistence.Interfaces;

namespace TravelApp.Business.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IAuthService _authService;
        private readonly PasswordHasher<User> _hasher;

        public UserService(IUserRepository repository, IAuthService authService)
        {
            _repository = repository;
            _hasher = new PasswordHasher<User>();
            _authService = authService;
        }
        public async Task<Response<object>> Register(RegisteringRequest registerRequest)
        {
            var user = new User
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                CreatedAt = DateTime.UtcNow,
            };
            var existingUser = await _repository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return ResponseFactory.Fail<object>("Email already exists.");
            }
            user.PasswordHash = _hasher.HashPassword(user,registerRequest.Password);
            await _repository.AddUserAsync(user);
            return ResponseFactory.Success<object>(
                new { user.Id, user.Username, user.Email },
                "User registered successfully."
            );
        }
        public async Task<Response<object>> Login(LoggingRequest request)
        {
            var user = await _repository.GetUser(request.Username);
            if (user == null)
                return ResponseFactory.Fail<object>("Invalid username or password.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                return ResponseFactory.Fail<object>("Invalid username or password.");

            var accessToken = _authService.CreateToken(user);
            var refreshToken = _authService.GenerateRefreshToken();

            await _authService.SaveRefreshToken(user.Id, refreshToken);

            var response = new
            {
                user.Id,
                user.Username,
                user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return ResponseFactory.Success<object>(response, "Login successful.");
        }

    }
}
