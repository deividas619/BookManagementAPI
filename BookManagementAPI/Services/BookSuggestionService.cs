using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services;

public class BookSuggestionService(IUserRepository userRepository, IReviewRepository reviewRepository)
    : IBookSuggestionService
{
    public async Task<List<Book>> GetBookSuggestionsAsync(Guid userId)
    {
        var userReviews = await reviewRepository.GetReviewsByUserId(userId);
        var allUsers = await userRepository.GetAllUsersAsync();
        var otherUsers = allUsers.Where(u => u.Id != userId).ToList();

        var distances = new List<(User, double)>();

        foreach (var other in otherUsers)
        {
            var otherReviews = await reviewRepository.GetReviewsByUserId(other.Id);


            var distance = CalculateEuclideanDistance(userReviews, otherReviews);
            distances.Add((other, distance));
        }

        var nearestUsers = distances.OrderBy(d => d.Item2).Take(3).Select(d => d.Item1).ToList();

        var suggestedBooks = new List<Book>();

        foreach (var user in nearestUsers)
        {
            var books = await reviewRepository.GetReviewsByUserId(user.Id);
            suggestedBooks.AddRange(books.Where(r => !userReviews.Select(ur => ur.BookId).Contains(r.BookId))
                .Select(r => r.Book)
                .Distinct());
        }

        return suggestedBooks.Distinct().ToList();
    }

    private double CalculateEuclideanDistance(List<Review> userReviews, List<Review> otherReviews)
    {
        var anyMatchingReviews = userReviews.Any(r => otherReviews.Any(or => or.BookId == r.BookId));
        if (!anyMatchingReviews)
            return double.MaxValue;

        double sumOfSquares = 0;
        foreach (var review in userReviews)
        {
            var otherReview = otherReviews.FirstOrDefault(r => r.BookId == review.BookId);
            if (otherReview != null)
            {
                double difference = review.Rating - otherReview.Rating;
                sumOfSquares += difference * difference;
            }
        }

        return Math.Sqrt(sumOfSquares);
    }
}