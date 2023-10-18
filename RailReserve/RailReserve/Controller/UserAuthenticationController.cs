using Microsoft.AspNetCore.Mvc;
using RailReserve.Dtos;
using RailReserve.Repository;
using System.Net;

namespace RailReserve.Controllers
{
    [ApiController]
    [Route("api/v1/userAuthenticate")]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAuthenticationController(IUserService userService) => _userService = userService;
       

        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            // call userService CreateAsync
            var result = await _userService.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            // call userService RegisterAsync
            var result = await _userService.RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

       
        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserLoginResponse))]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            // call userService LoginAsync
            var result = await _userService.LoginAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);

        }

      
    }
}
