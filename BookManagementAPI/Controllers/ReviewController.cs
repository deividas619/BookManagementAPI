using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ReviewController(IBookRepository bookRepository, IReviewService reviewService) : ControllerBase
{
    [HttpPost("AddReview")]
    [Authorize(Roles = nameof(UserRole.Admin) + ", " + nameof(UserRole.Regular))]
    public async Task<ActionResult<Review>> AddReview([FromQuery] Guid bookId, [FromBody] ReviewDto reviewDto)
    {
        if (reviewDto.Rating is < 1 or > 5)
        {
            return BadRequest("Rating must be between 1 and 5");
        }

        var userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        var result = await reviewService.AddReview(bookId, reviewDto, userName);

        if (result.Text == "Not found")
            return BadRequest("No book match was found!");
        return Ok(result);
    }

    [HttpGet("GetReviewsByBookId")]
    [AllowAnonymous]
    public async Task<ActionResult<Book>> GetReviewsByBookId(Guid bookId)
    {
        var reviews = await reviewService.GetReviewsByBookId(bookId);
        if (reviews is null)
        {
            return BadRequest("No book reviews were found!");
        }
        return Ok(reviews);
    }

    [HttpDelete("RemoveReviewById")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<Review>> RemoveReviewById([FromQuery] Guid reviewId)
    {
        var result = await reviewService.RemoveReviewById(reviewId);

        if (result.Text == "Not found")
            return BadRequest("No review match was found!");
        return Ok(result);
    }
}