using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookManagementAPI.DTOs;
using BookManagementAPI.Services;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public ActionResult<ResponseDto> Login(string username, string password)
        {
            var response = _userService.Login(username, password);
            if (!response.IsSuccess)
                return BadRequest(response.Message);
            return Ok(_jwtService.GetJwtToken(username, response.Role));
        }

        [HttpPost("Signup")]
        [AllowAnonymous]
        public ActionResult<ResponseDto> Signup([FromBody] UserDto request)
        {
            var response = _userService.Signup(request.Username, request.Password);
            if (!response.IsSuccess)
                return BadRequest(response.Message);
            return response;
        }

        [HttpGet("ChangePassword")]
        [Authorize]
        public ActionResult<ResponseDto> ChangePassword(string username, string oldPassword, string newPassword, string newPasswordAgain)
        {
            var response = _userService.ChangePassword(username, oldPassword, newPassword, newPasswordAgain);
            if (!response.IsSuccess)
                return BadRequest(response.Message);
            return response;
        }

        [HttpGet("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ResponseDto> ChangeRole(string username, string newRole)
        {
            var response = _userService.ChangeRole(username, newRole);
            if (!response.IsSuccess)
                return BadRequest(response.Message);
            return response;
        }
    }
}
