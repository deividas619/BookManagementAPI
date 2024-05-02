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
        var allReviews = await reviewRepository.GetReviewsAsync();
        var userReviews = allReviews.Where(r => r.CreatedByUserId == userId)
            .ToList();

        var otherReviews = allReviews.Where(r => r.CreatedByUserId != userId)
            .GroupBy(r => r.CreatedByUserId);

        var distances = otherReviews.Select(group =>
                (group.Key, CalculateEuclideanDistance(userReviews, group.ToList())))
            .OrderBy(d => d.Item2)
            .Take(3)
            .ToList();

        var nearestUserIds = distances.Select(d => d.Key)
            .ToList();

        var suggestedBooks = new HashSet<Book>();

        foreach (var nearestUserId in nearestUserIds)
        {
            var books = allReviews.Where(r =>
                    r.CreatedByUserId == nearestUserId && userReviews.All(ur => ur.BookId != r.BookId))
                .Select(r => r.Book)
                .Distinct();

            suggestedBooks.UnionWith(books);
        }

        return suggestedBooks.ToList();
    }

    private double CalculateEuclideanDistance(List<Review> userReviews, List<Review> otherReviews)
    {
        double sumOfSquares = 0;
        var foundMatching = false;

        foreach (var review in userReviews)
        {
            var otherReview = otherReviews.FirstOrDefault(r => r.BookId == review.BookId);
            if (otherReview != null)
            {
                foundMatching = true;
                double difference = review.Rating - otherReview.Rating;
                sumOfSquares += difference * difference;
            }
        }

        return foundMatching ? Math.Sqrt(sumOfSquares) : double.MaxValue;
    }
}