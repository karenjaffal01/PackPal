using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Domain.Common;
using TravelApp.Persistence.Repositories;

namespace TravelApp.Business.Services
{
    public class UserService
    {
       private readonly UserRepository _repository;
       public UserService(UserRepository repository)
       {
            _repository = repository;
       }
       public Task<Response<object>> Register (RegisterRequest registerRequest)
        {

        }
    }
}
