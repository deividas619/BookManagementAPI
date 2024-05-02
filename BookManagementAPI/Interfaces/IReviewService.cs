using System;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces;

public interface IReviewService
{
    Task<Review> AddReview(Guid bookId, ReviewDto reviewDto, string userName);
    Task<Book> GetReviewsByBookId(Guid bookId);
    Task<Review> RemoveReviewById(Guid reviewId);
}