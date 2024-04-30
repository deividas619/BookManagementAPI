using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using BookManagementAPI.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BookManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IBookRepository _bookRepository;

    public ReviewController(IReviewService reviewService, IBookRepository bookRepository)
    {
        _reviewService = reviewService;
        _bookRepository = bookRepository;
    }


    [HttpPost("AddReview")]
    [Authorize(Roles = "Admin, Regular")]
    public async Task<ActionResult<Review>> AddReview([FromBody] ReviewDto reviewDto)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

        if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
        {
            return BadRequest("Rating must be between 1 and 5.");
        }

        // Check if the book with the specified title exists
        var filter = new SearchFilterDto { Title = reviewDto.BookTitle };
        var existingBooks = await _bookRepository.GetBooksByFilter(filter, 0, 1);
        var existingBook = existingBooks.FirstOrDefault();

        if (existingBook == null)
        {
            return BadRequest($"Book with title '{reviewDto.BookTitle}' does not exist.");
        }

        var result = await _reviewService.AddReview(reviewDto, userName);

        if (result == null)
            return BadRequest("Failed to add a review!");
        return Ok(result);
    }


    [HttpDelete("RemoveReviewById")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Review>> RemoveReviewById([FromQuery] Guid id)
    {
        var result = await _reviewService.RemoveReviewById(id);

        if (result == null)
            return BadRequest("Failed to delete the review!");

        return Ok(result);
    }


    [HttpGet("GetAllReviews")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
    {
        var result = await _reviewService.GetAllReviews();
        if (!result.Any())
            return BadRequest("There are no reviews in the database!");
        return Ok(result);
    }

    [HttpGet("GetReviewsAndAverageRatingForBook")]
    [AllowAnonymous]
    public async Task<ActionResult<(IEnumerable<Review> reviews, double averageRating)>> GetReviewsAndAverageRatingForBook(string bookTitle)
    {
        var result = await _reviewService.GetReviewsAndAverageRatingForBook(bookTitle);
        if (result.Item1 == null)
            return BadRequest($"No reviews found for the book with title '{bookTitle}'!");
        return Ok(result.Item1);
    }

    [HttpGet("GetAverageRatingForBook")]
    [AllowAnonymous]
    public async Task<ActionResult<double>> GetAverageRatingForBook(string bookTitle)
    {
        var averageRating = await _reviewService.GetAverageRatingForBook(bookTitle);
        if (averageRating == null)
            return BadRequest($"No reviews found for the book with title '{bookTitle}'!");
        return Ok("Average rating of the book is " + averageRating);
    }

}