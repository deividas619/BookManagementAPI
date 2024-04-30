using BookManagementAPI.DTOs;
using BookManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //[HttpPost("create")]//delete
        //public ActionResult CreateAdmin([FromBody] AdminCreateDto adminDto)
        //{
        //    var response = _adminService.CreateAdminAsync(adminDto.Username, adminDto.Password);
        //    return Ok(response);
        //}

        [HttpPut("{username}/set")]
        public async Task<IActionResult> SetAdmin(string username, string newRole)
        {
            var response = await _adminService.SetAdminAndChangeRoleAsync(username, newRole);
            return Ok(response);
        }

        [HttpDelete("{username}/revoke")]
        public async Task<ActionResult> RevokeAdmin(string username, string newRole)
        {
            var response = await _adminService.RevokeAdminAndChangeRoleAsync(username, newRole);
            return Ok(response);
        }


        //[HttpDelete("review/{reviewId}/delete")]
        //public ActionResult DeleteReview(int reviewId)
        //{
        //    var response = _adminService.DeleteReview(reviewId);
        //    return Ok(response);
        //}


    }
}
