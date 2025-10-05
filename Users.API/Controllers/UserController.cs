using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Business.Interfaces;
using TravelApp.Domain.Requests;


namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisteringRequest request)
        {
            var response = await _service.Register(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
