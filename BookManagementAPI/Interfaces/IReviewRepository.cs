using System;
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
    }
}