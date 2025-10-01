using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Entities;

namespace TravelApp.Business.Interfaces
{
    public interface IAuthService
    {
        public string CreateToken(User user);
        public string generateRefreshToken();
    }
}
