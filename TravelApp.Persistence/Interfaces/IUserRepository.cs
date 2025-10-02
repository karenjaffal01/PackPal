using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Entities;

namespace TravelApp.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
    }
}
