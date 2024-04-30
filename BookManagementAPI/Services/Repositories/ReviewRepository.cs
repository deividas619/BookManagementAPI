using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookManagementAPI.Services.Repositories;

public class ReviewRepository(ApplicationDbContext context) : IReviewRepository
{
    public async Task<IEnumerable<Review>> GetAllReviews()
    {
        try
        {
            return await context.Reviews.ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetAllReviews)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Review> AddReview(Book book, Review review)
    {
        try
        {
            //book.Reviews.Add(review);
            context.Reviews.Add(review);//istrinti book
            await context.SaveChangesAsync();
            return review;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(AddReview)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Review> RemoveReviewById(Guid id)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review == null)
        {
            Log.Error($"[{nameof(RemoveReviewById)}]: Review not found!");
            return null;
        }

        try
        {
            context.Reviews.Remove(review);
            await context.SaveChangesAsync();
            return review;
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(RemoveReviewById)}]: {e.Message}");
            throw;
        }
    }


    public Guid GetUserId(string username)
    {
        return context.Users.SingleOrDefault(x => x.Username == username).Id;
    }

    public async Task<IEnumerable<Review>> GetReviewsByBookTitle(string bookTitle)
    {
        try
        {
            return await context.Reviews.Where(r => r.BookTitle == bookTitle).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetReviewsByBookTitle)}]: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Review>> GetReviewsByBookId(Guid bookId)
    {
        return await context.Reviews.Where(r => r.Id == bookId).ToListAsync();
    }

}