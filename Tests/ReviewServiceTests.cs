using BookManagementAPI.DTOs;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using Moq;

namespace Tests
{
    public class ReviewServiceTests
    {
        [Fact]
        public async Task AddReview_ReturnsReview_WhenReviewAddedSuccessfully()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var userName = "testUser";
            var reviewDto = new ReviewDto { Text = "Test review", Rating = 5 };
            var mockBookRepository = new Mock<IBookRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var reviewService = new ReviewService(mockBookRepository.Object, mockReviewRepository.Object);

            mockBookRepository.Setup(repo => repo.GetBookById(It.IsAny<Guid>())).ReturnsAsync(new Book());
            mockReviewRepository.Setup(repo => repo.AddReview(It.IsAny<Book>(), It.IsAny<Review>()))
                .ReturnsAsync(new Review { Text = reviewDto.Text, Rating = reviewDto.Rating });

            // Act
            var result = await reviewService.AddReview(bookId, reviewDto, userName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reviewDto.Text, result.Text);
            Assert.Equal(reviewDto.Rating, result.Rating);
        }

        [Fact]
        public async Task AddReview_ReturnsNull_WhenBookNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var reviewDto = new ReviewDto { Text = "Test review", Rating = 5 };
            var mockBookRepository = new Mock<IBookRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var reviewService = new ReviewService(mockBookRepository.Object, mockReviewRepository.Object);

            mockBookRepository.Setup(repo => repo.GetBookById(It.IsAny<Guid>())).ReturnsAsync((Book)null);

            // Act
            var result = await reviewService.AddReview(bookId, reviewDto, "testUser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetReviewsByBookId_ReturnsNull_WhenBookNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var mockBookRepository = new Mock<IBookRepository>();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var reviewService = new ReviewService(mockBookRepository.Object, mockReviewRepository.Object);

            mockBookRepository.Setup(repo => repo.GetBookById(It.IsAny<Guid>())).ReturnsAsync((Book)null);

            // Act
            var result = await reviewService.GetReviewsByBookId(bookId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveReviewById_ReturnsReview_WhenReviewExists()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var reviewService = new ReviewService(null, mockReviewRepository.Object);

            mockReviewRepository.Setup(repo => repo.GetReviewById(It.IsAny<Guid>()))
                .ReturnsAsync(new Review { Id = reviewId, Text = "Test review", Rating = 5 });
            mockReviewRepository.Setup(repo => repo.RemoveReviewById(It.IsAny<Guid>()))
                .ReturnsAsync(new Review { Id = reviewId, Text = "Test review", Rating = 5 });

            // Act
            var result = await reviewService.RemoveReviewById(reviewId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reviewId, result.Id);
            Assert.Equal("Test review", result.Text);
            Assert.Equal(5, result.Rating);
        }

        [Fact]
        public async Task RemoveReviewById_ReturnsReview_WhenRemovalIsSuccessful()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var mockReviewRepository = new Mock<IReviewRepository>();
            var reviewService = new ReviewService(null, mockReviewRepository.Object);
            var reviewToRemove = new Review { Id = reviewId, Text = "Test review", Rating = 5 };

            mockReviewRepository.Setup(repo => repo.GetReviewById(It.IsAny<Guid>())).ReturnsAsync(reviewToRemove);
            mockReviewRepository.Setup(repo => repo.RemoveReviewById(It.IsAny<Guid>())).ReturnsAsync(reviewToRemove);

            // Act
            var result = await reviewService.RemoveReviewById(reviewId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reviewId, result.Id);
            Assert.Equal("Test review", result.Text);
            Assert.Equal(5, result.Rating);
        }
    }
}