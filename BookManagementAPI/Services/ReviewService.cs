using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BookManagementAPI.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;

    public ReviewService(IReviewRepository reviewRepository, IBookRepository bookRepository)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
    }
    public async Task<Review> AddReview(ReviewDto reviewDto, string userName)
    {
        try
        {
            var review = new Review
            {
                Text = reviewDto.Text,
                Rating = reviewDto.Rating,
                BookTitle = reviewDto.BookTitle,
                CreatedByUserId = _reviewRepository.GetUserId(userName),
            };
            return await _reviewRepository.AddReview(review);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(AddReview)}]: {e.Message}");
            throw;
        }
    }
    public async Task<Review> RemoveReviewById(Guid id)
    {
        try
        {
            return await _reviewRepository.RemoveReviewById(id);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(RemoveReviewById)}]: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Review>> GetAllReviews()
    {
        try
        {
            return await _reviewRepository.GetAllReviews();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetAllReviews)}]: {e.Message}");
            throw;
        }
    }
    public async Task<(IEnumerable<Review>, double)> GetReviewsAndAverageRatingForBook(string bookTitle)
    {
        try
        {
            var reviews = await _reviewRepository.GetReviewsByBookTitle(bookTitle);
            var averageRating = CalculateAverageRating(reviews);
            return (reviews, averageRating);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetReviewsAndAverageRatingForBook)}]: {e.Message}");
            throw;
        }
    }

    public async Task<double?> GetAverageRatingForBook(string bookTitle)
    {
        var reviews = await _reviewRepository.GetReviewsByBookTitle(bookTitle);
        if (reviews == null || !reviews.Any())
        {
            return null; // No reviews found for the book
        }

        double averageRating = reviews.Average(r => r.Rating);
        return averageRating;
    }

    private double CalculateAverageRating(IEnumerable<Review> reviews)
    {
        if (reviews == null || !reviews.Any())
        {
            return 0;
        }

        double totalRating = 0;
        foreach (var review in reviews)
        {
            totalRating += review.Rating;
        }

        return totalRating / reviews.Count();
    }
}