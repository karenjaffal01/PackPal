using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Dto;
using TravelApp.Domain.Entities;
using TravelApp.Domain.Requests;

namespace TravelApp.Business.Interfaces
{
    public interface IAuthService
    {
        public string CreateToken(User user);
        public string GenerateRefreshToken();
        Task SaveRefreshToken(int userId, string refreshToken);
        Task<RefreshTokenDto?> ValidateRefreshToken(RefreshTokenRequest request);
    }
}
