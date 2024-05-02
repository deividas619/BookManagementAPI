using System;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Serilog;

namespace BookManagementAPI.Services;

public class ReviewService(IBookRepository bookRepository, IReviewRepository reviewRepository) : IReviewService
{
    public async Task<Review> AddReview(Guid bookId, ReviewDto reviewDto, string userName)
    {
        try
        {
            var book = await bookRepository.GetBookById(bookId);
            if (book is not null)
            {
                var review = new Review
                {
                    Text = reviewDto.Text,
                    Rating = reviewDto.Rating,
                    BookId = bookId,
                    CreatedByUserId = bookRepository.GetUserId(userName),
                };

                return await reviewRepository.AddReview(book, review);
            }
            else
            {
                Log.Error($"[{nameof(AddReview)}]: No Book match was found with id: {bookId}!");
                return new Review { Text = "Not found" };
            }
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(AddReview)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> GetReviewsByBookId(Guid bookId)
    {
        var book = await bookRepository.GetBookById(bookId);
        if (book is not null)
        {
            try
            {
                return await reviewRepository.GetReviewsByBookId(bookId);
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(GetReviewsByBookId)}]: {e.Message}");
                throw;
            }
        }
        else
        {
            Log.Error($"[{nameof(GetReviewsByBookId)}]: No Book match was found with id: {bookId}!");
            return null;
        }
    }
    public async Task<Review> RemoveReviewById(Guid reviewId)
    {
        var currentReview = await reviewRepository.GetReviewById(reviewId);
        if (currentReview is not null)
        {
            try
            {
                return await reviewRepository.RemoveReviewById(reviewId);
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(RemoveReviewById)}]: {e.Message}");
                throw;
            }
        }
        else
        {
            Log.Error($"[{nameof(RemoveReviewById)}]: No review was found with id: {reviewId}!");
            return new Review { Text = "Not found" };
        }
    }
}