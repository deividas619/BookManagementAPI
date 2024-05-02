using BookManagementAPI.Controllers;
using BookManagementAPI.DTOs;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class ReviewControllerTests
    {
        [Fact]
        public async Task AddReview_ReturnsOk_WhenReviewAddedSuccessfully()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var mockReviewService = new Mock<IReviewService>();
            mockReviewService.Setup(service =>
                    service.AddReview(It.IsAny<Guid>(), It.IsAny<ReviewDto>(), It.IsAny<string>()))
                .ReturnsAsync(new Review());
            var controller = new ReviewController(null, mockReviewService.Object);
            var reviewDto = new ReviewDto { Text = "Test review", Rating = 5 };

            // Act
            var result = await controller.AddReview(bookId, reviewDto);

            // Assert
            Assert.IsType<OkResult>(result.Result);
        }

        [Fact]
        public async Task AddReview_ReturnsBadRequest_WhenRatingIsInvalid()
        {
            // Arrange
            var mockReviewService = new Mock<IReviewService>();
            var controller = new ReviewController(null, mockReviewService.Object);
            var reviewDto = new ReviewDto { Text = "Test review", Rating = 6 };

            // Act
            var result = await controller.AddReview(Guid.NewGuid(), reviewDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Rating must be between 1 and 5", badRequestResult.Value);
        }

        [Fact]
        public async Task AddReview_ReturnsBadRequest_WhenBookNotFound()
        {
            // Arrange
            var mockReviewService = new Mock<IReviewService>();
            mockReviewService.Setup(service =>
                    service.AddReview(It.IsAny<Guid>(), It.IsAny<ReviewDto>(), It.IsAny<string>()))
                .ReturnsAsync(new Review { Text = "Not found" });
            var controller = new ReviewController(null, mockReviewService.Object);
            var reviewDto = new ReviewDto { Text = "Test review", Rating = 5 };

            // Act
            var result = await controller.AddReview(Guid.NewGuid(), reviewDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("No book match was found!", badRequestResult.Value);
        }

        [Fact]
        public async Task AddReview_ReturnsBadRequest_WhenRatingIsOutOfRange()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var mockReviewService = new Mock<IReviewService>();
            var controller = new ReviewController(null, mockReviewService.Object);
            var reviewDto = new ReviewDto { Text = "Test review", Rating = 0 };

            // Act
            var result = await controller.AddReview(bookId, reviewDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetReviewsByBookId_ReturnsBadRequest_WhenBookNotFound()
        {
            // Arrange
            var mockReviewService = new Mock<IReviewService>();
            mockReviewService.Setup(service => service.GetReviewsByBookId(It.IsAny<Guid>()))
                .ReturnsAsync((Book)null);
            var controller = new ReviewController(null, mockReviewService.Object);

            // Act
            var result = await controller.GetReviewsByBookId(Guid.NewGuid());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("No book reviews were found!", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveReviewById_ReturnsOk_WhenReviewRemovedSuccessfully()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var mockReviewService = new Mock<IReviewService>();
            mockReviewService.Setup(service => service.RemoveReviewById(It.IsAny<Guid>()))
                .ReturnsAsync(new Review { Text = "Review removed successfully" });
            var controller = new ReviewController(null, mockReviewService.Object);

            // Act
            var result = await controller.RemoveReviewById(reviewId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var review = Assert.IsAssignableFrom<Review>(okResult.Value);
            Assert.Equal("Review removed successfully", review.Text);
        }

        [Fact]
        public async Task RemoveReviewById_ReturnsBadRequest_WhenReviewNotFound()
        {
            // Arrange
            var mockReviewService = new Mock<IReviewService>();
            mockReviewService.Setup(service => service.RemoveReviewById(It.IsAny<Guid>()))
                .ReturnsAsync(new Review { Text = "Not found" });
            var controller = new ReviewController(null, mockReviewService.Object);

            // Act
            var result = await controller.RemoveReviewById(Guid.NewGuid());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("No review match was found!", badRequestResult.Value);
        }
    }
}