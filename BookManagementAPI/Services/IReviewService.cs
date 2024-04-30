using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Services;


public interface IReviewService
{
    Task<(IEnumerable<Review>, double)> GetReviewsAndAverageRatingForBook(string bookTitle);
    Task<IEnumerable<Review>> GetReviewsByBookTitle(string bookTitle);
    Task<IEnumerable<Review>> GetReviewsByBookId(Guid bookId);
    Task<double?> GetAverageRatingForBook(string bookTitle);
    Task<IEnumerable<Review>> GetAllReviews();
    Task<Review> AddReview(Book book, ReviewDto reviewDto, string userName);
    Task<Review> RemoveReviewById(Guid id);
}