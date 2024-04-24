using Microsoft.AspNetCore.Mvc;
using Moq;
using BookManagementAPI.Controllers;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services;

namespace Tests
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BookController(_mockBookService.Object);
        }

        [Fact]
        public void GetBooks_WithValidTitle_ReturnsOkObjectResult()
        {
            // Arrange
            string title = "Title";

            // Act
            var result = _controller.GetBooks(title);

            // Assert
            Assert.IsType<ActionResult<Book>>(result);
        }

        [Fact]
        public void RemoveBook_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            _controller.RemoveBook(id);

            // Assert
            _mockBookService.Verify(service => service.RemoveBook(id), Times.Once);
        }

        [Fact]
        public void AddBook_WithValidBook_ReturnsOkObjectResult()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Book Title", Genre = new Genre{ Id = Guid.NewGuid(), Name = "Fiction" } };
            var responseDto = new ResponseDto(isSuccess: true, message: "Book added successfully");
            _mockBookService.Setup(service => service.AddBook(It.IsAny<Book>())).Returns(responseDto);

            // Act
            var result = _controller.AddBook(book);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto>>(result);
            Assert.NotNull(actionResult.Value);
            var resultDto = Assert.IsType<ResponseDto>(actionResult.Value);
            Assert.True(resultDto.IsSuccess);
            Assert.Equal("Book added successfully", resultDto.Message);
        }
    }
}