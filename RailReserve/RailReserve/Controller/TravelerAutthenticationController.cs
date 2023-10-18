using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RailReserve.Dtos;
using RailReserve.Model;
using RailReserve.Service;
using System.Net;

namespace RailReserve.Controllers
{
    [ApiController]
    [Route("api/v1/travelerAuthenticate")]
    public class TravelerAutthenticationController : ControllerBase
    {
        private readonly ITravelerService _travelerService;

        public TravelerAutthenticationController(ITravelerService travelerService) => _travelerService = travelerService;
     

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] TravelerRegisterRequest request)
        {
            var result = await _travelerService.RegisterAsync(request);

            return result.Success ? Ok(result) : BadRequest(result);

        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TravelerLoginResponse))]
        public async Task<IActionResult> Login([FromBody] TravelerLoginRequest request)
        {
            var result = await _travelerService.LoginAsync(request);

            return result.Success ? Ok(result) : BadRequest(result.Message);


        }
    }
}
