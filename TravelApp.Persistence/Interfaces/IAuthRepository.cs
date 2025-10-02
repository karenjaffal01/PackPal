using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Entities;

namespace TravelApp.Persistence.Interfaces
{
    public interface IAuthRepository
    {
        string generateRefreshToken();
        Task SaveRefreshToken(int userId, string refreshToken);
        Task<RefreshToken> GetRefreshToken(int userId);
    }
}
