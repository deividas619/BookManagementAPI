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
        public async Task GetAllBooks_ReturnsOkResult_WithAListOfBooks()
        {
            // Arrange
            var books = new List<Book> { new Book(), new Book() };
            _mockBookService.Setup(s => s.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBooks = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(2, returnBooks.Count);
        }

        [Fact]
        public async Task GetBookByTitle_ReturnsOkResult_WithAListOfBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book> { new Book(), new Book() };
            _mockBookService.Setup(s => s.GetBooksByTitle(It.IsAny<string>())).ReturnsAsync(books);

            // Act
            var result = await _controller.GetBookByTitle("Test");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBooks = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(2, returnBooks.Count);
        }

        [Fact]
        public async Task GetBookByTitle_ReturnsBadRequest_WhenNoBooksExist()
        {
            // Arrange
            _mockBookService.Setup(s => s.GetBooksByTitle(It.IsAny<string>())).ReturnsAsync(new List<Book>());

            // Act
            var result = await _controller.GetBookByTitle("Test");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("There are no books matching the title!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult_WithBook_WhenBookExists()
        {
            // Arrange
            var book = new Book();
            _mockBookService.Setup(s => s.GetBookById(It.IsAny<Guid>())).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBookById(Guid.NewGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(book, returnBook);
        }

        [Fact]
        public async Task GetBookById_ReturnsBadRequest_WhenBookDoesNotExist()
        {
            // Arrange
            _mockBookService.Setup(s => s.GetBookById(It.IsAny<Guid>())).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.GetBookById(Guid.NewGuid());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("There are no books matching the id!", badRequestResult.Value);
        }

        [Fact]
        public async Task AddBook_ReturnsOkResult_WithBook_WhenAdditionIsSuccessful()
        {
            // Arrange
            var book = new Book();
            _mockBookService.Setup(s => s.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<Genre>())).ReturnsAsync(book);

            // Act
            var result = await _controller.AddBook(new BookDto());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(book, returnBook);
        }

        [Fact]
        public async Task AddBook_ReturnsBadRequest_WhenAdditionFails()
        {
            // Arrange
            _mockBookService.Setup(s => s.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<Genre>())).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.AddBook(new BookDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to add a book!", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOkResult_WithBook_WhenUpdateIsSuccessful()
        {
            // Arrange
            var book = new Book();
            _mockBookService.Setup(s => s.UpdateBook(It.IsAny<Book>())).ReturnsAsync(book);

            // Act
            var result = await _controller.UpdateBook(new Book());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(book, returnBook);
        }

        [Fact]
        public async Task UpdateBook_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            _mockBookService.Setup(s => s.UpdateBook(It.IsAny<Book>())).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.UpdateBook(new Book());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to update a book!", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveBookById_ReturnsOkResult_WithBook_WhenRemovalIsSuccessful()
        {
            // Arrange
            var book = new Book();
            _mockBookService.Setup(s => s.RemoveBookById(It.IsAny<Guid>())).ReturnsAsync(book);

            // Act
            var result = await _controller.RemoveBookById(Guid.NewGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(book, returnBook);
        }

        [Fact]
        public async Task RemoveBookById_ReturnsBadRequest_WhenRemovalFails()
        {
            // Arrange
            _mockBookService.Setup(s => s.RemoveBookById(It.IsAny<Guid>())).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.RemoveBookById(Guid.NewGuid());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to delete a book!", badRequestResult.Value);
        }
    }
}