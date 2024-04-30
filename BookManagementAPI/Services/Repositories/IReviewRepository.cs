using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviews();
        Task<IEnumerable<Review>> GetReviewsByBookTitle(string bookTitle);
        Task<Review> AddReview(Review review);
        Task<Review> RemoveReviewById(Guid id);
        Guid GetUserId(string userName);
    }
}
