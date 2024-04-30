using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService, IJwtService jwtService) : ControllerBase
{
    [HttpPost("Login")]
    [AllowAnonymous]
    public ActionResult<ResponseDto> Login(string username, string password)
    {
        var response = userService.Login(username, password);
        if (!response.IsSuccess)
            return BadRequest(response.Message);
        if (response.Role.HasValue)
        {
            var token = jwtService.GetJwtToken(username, response.Role.Value);
            return Ok(new { Token = token });
        }
        return BadRequest("Role information is missing or invalid.");
    }

    [HttpPost("Signup")]
    [AllowAnonymous]
    public ActionResult<ResponseDto> Signup([FromBody] UserDto request)
    {
        var response = userService.Signup(request.Username, request.Password);
        if (!response.IsSuccess)
            return BadRequest(response.Message);
        return response;
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public ActionResult<ResponseDto> ChangePassword(string username, string oldPassword, string newPassword,
        string newPasswordAgain)
    {
        var response = userService.ChangePassword(username, oldPassword, newPassword, newPasswordAgain);
        if (!response.IsSuccess)
            return BadRequest(response.Message);
        return response;
    }

    [HttpPost("ChangeRole")]
    [Authorize(Roles = nameof(UserRole.Admin))]

    public ActionResult<ResponseDto> ChangeRole(string username, UserRole newRole)
    {
        var response = userService.ChangeRole(username, newRole);
        if (!response.IsSuccess)
            return BadRequest(response.Message);
        return response;
    }
}