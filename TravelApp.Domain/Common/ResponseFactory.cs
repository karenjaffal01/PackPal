using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Domain.Common
{
    public static class ResponseFactory
    {
        public static Response<T> Success<T>(T data, string message) =>
            new() { Success = true, Message = message, Data = data };

        public static Response<T> Fail<T>(string message) =>
            new() { Success = false, Message = message, Data = default! };
    }
}
