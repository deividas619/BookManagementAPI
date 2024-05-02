using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookManagementAPI.Repositories;

public class ReviewRepository(ApplicationDbContext context) : IReviewRepository
{
    public async Task<Review> AddReview(Book book, Review review)
    {
        try
        {
            context.Reviews.Add(review);
            book.AverageRating = CalculateAvgRating(book);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(AddReview)}]: {e.Message}");
            throw;
        }

        Log.Information($"[{nameof(AddReview)}]: Successfully added review: {review.Id}");

        return review;
    }

    public async Task<Book> GetReviewsByBookId(Guid bookId)
    {
        try
        {
            var output = await context.Books.Include(b => b.Reviews).FirstOrDefaultAsync(b => b.Id == bookId);

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetReviewsByBookId)}]: {e.Message}");
            throw;
        }
    }

    public async Task<List<Review>> GetReviewsByUserId(Guid userId)
    {
        try
        {
            return await context.Reviews.Include(b => b.Book).Where(r => r.CreatedByUserId == userId).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetReviewsByUserId)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Review> GetReviewById(Guid id)
    {
        try
        {
            var output = await context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

            return output;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetReviewById)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Review> RemoveReviewById(Guid reviewId)
    {
        var reviewToRemove = await context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        try
        {
            context.Reviews.Remove(reviewToRemove);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(RemoveReviewById)}]: {e.Message}");
            throw;
        }

        Log.Information($"[{nameof(RemoveReviewById)}]: Successfully removed review with id: {reviewId}");

        return reviewToRemove;
    }

    public async Task<List<Review>> GetReviewsAsync()
    {
        try
        {
            return await context.Reviews.Include(r => r.Book).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetReviewsAsync)}]: {e.Message}");
            throw;
        }
    }

    private decimal CalculateAvgRating(Book book)
    {
        return (decimal)book.Reviews.Sum(r => r.Rating) / book.Reviews.Count();
    }
}