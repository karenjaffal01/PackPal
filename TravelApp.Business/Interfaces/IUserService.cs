using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Common;
using TravelApp.Domain.Requests;

namespace TravelApp.Business.Interfaces
{
    public interface IUserService
    {
        Task<Response<object>> Register(RegisteringRequest registerRequest);
    }
}
