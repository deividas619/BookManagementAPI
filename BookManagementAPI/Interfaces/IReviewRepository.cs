using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> AddReview(Book book, Review review);
        Task<Book> GetReviewsByBookId(Guid bookId);
        Task<Review> GetReviewById(Guid id);
        Task<Review> RemoveReviewById(Guid reviewId);
        Task<List<Review>> GetReviewsByUserId(Guid userId);
        Task<List<Review>> GetReviewsAsync();
    }
}