using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Domain.Requests
{
    public class RefreshTokenRequest
    {
        public int userId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;

    }
}
