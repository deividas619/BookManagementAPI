using Microsoft.AspNetCore.Mvc;
using Moq;
using BookManagementAPI.Controllers;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Tests
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _bookController;

        public BookControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _bookController = new BookController(_mockBookService.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "RegularUser"),
                new Claim(ClaimTypes.Role, "Regular")
            }));
            _bookController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOk_WhenBooksExist()
        {
            // Arrange
            var mockBooks = new List<Book> { new Book { Id = Guid.NewGuid(), Title = "Book 1" } };
            _mockBookService.Setup(service => service.GetAllBooks()).ReturnsAsync(mockBooks);

            // Act
            var result = await _bookController.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Single(books);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsBadRequest_WhenNoBooksExist()
        {
            // Arrange
            _mockBookService.Setup(service => service.GetAllBooks()).ReturnsAsync(new List<Book>());

            // Act
            var result = await _bookController.GetAllBooks();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("There are no books in the database!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetBooksByFilter_ReturnsOk_WhenBooksExist()
        {
            // Arrange
            var mockBooks = new List<Book> { new Book { Id = Guid.NewGuid(), Title = "Book 1" } };
            _mockBookService.Setup(service => service.GetBooksByFilter(It.IsAny<SearchFilterDto>(), 0, 0)).ReturnsAsync(mockBooks);

            // Act
            var result = await _bookController.GetBooksByFilter("Title", "Author", new string[] { "Genre" }, new DateOnly(), new DateOnly());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Single(books);
        }

        [Fact]
        public async Task GetBooksByFilter_ReturnsOk_WhenNoBooksExist()
        {
            // Arrange
            _mockBookService.Setup(service => service.GetBooksByFilter(It.IsAny<SearchFilterDto>(), 0, 0))
                .ReturnsAsync(new List<Book>());

            // Act
            var result = await _bookController.GetBooksByFilter(null, null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Empty((IEnumerable<Book>)okResult.Value);
        }

        [Fact]
        public async Task AddBook_ReturnsOk_WhenBookAddedSuccessfully()
        {
            // Arrange
            var bookDto = new BookDto { Title = "New Book", Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };
            _mockBookService.Setup(service => service.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<GenreDto>(), It.IsAny<string>()))
                .ReturnsAsync(new Book { Id = Guid.NewGuid(), Title = "New Book" });

            // Act
            var result = await _bookController.AddBook(bookDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var addedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal("New Book", addedBook.Title);
        }

        [Fact]
        public async Task AddBook_ReturnsBadRequest_WhenAddingBookFails()
        {
            // Arrange
            var bookDto = new BookDto { Title = "New Book", Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };
            _mockBookService.Setup(service => service.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<GenreDto>(), It.IsAny<string>()))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _bookController.AddBook(bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to add a book!", badRequestResult.Value);
        }

        [Fact]
        public async Task AddBook_ReturnsBadRequest_WhenTitleIsNull()
        {
            // Arrange
            var bookDto = new BookDto { Title = null, Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };

            // Act
            var result = await _bookController.AddBook(bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to add a book!", badRequestResult.Value);
        }

        [Fact]
        public async Task AddBook_ReturnsForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            var bookDto = new BookDto { Title = "New Book", Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };

            // Act
            var result = await _bookController.AddBook(bookDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOk_WhenBookUpdatedSuccessfully()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDto = new BookDto { Title = "Updated Book", Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };
            var currentBook = new Book { Id = bookId, Title = "Existing Book" };
            _mockBookService.Setup(service => service.UpdateBook(It.IsAny<Guid>(), It.IsAny<BookDto>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Book { Id = bookId, Title = "Updated Book" });

            // Act
            var result = await _bookController.UpdateBook(bookId, bookDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal("Updated Book", updatedBook.Title);
        }

        [Fact]
        public async Task UpdateBook_ReturnsForbidden_WhenUserIsNotAdminOrCreator()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDto = new BookDto { Title = "Updated Book", Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };

            // Act
            var result = await _bookController.UpdateBook(bookId, bookDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task RemoveBookById_ReturnsOk_WhenBookRemovedSuccessfully()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookToRemove = new Book { Id = bookId, Title = "Book to Remove" };
            _mockBookService.Setup(service => service.RemoveBookById(It.IsAny<Guid>()))
                .ReturnsAsync(bookToRemove);

            // Act
            var result = await _bookController.RemoveBookById(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var removedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal("Book to Remove", removedBook.Title);
        }

        [Fact]
        public async Task RemoveBookById_ReturnsBadRequest_WhenBookNotFound()
        {
            // Arrange
            var nonExistingBookId = Guid.NewGuid();
            _mockBookService.Setup(service => service.RemoveBookById(It.IsAny<Guid>()))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _bookController.RemoveBookById(nonExistingBookId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal($"Failed to delete a book!", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveBookById_ReturnsBadRequest_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookController.ControllerContext.HttpContext.User = null;

            // Act
            var result = await _bookController.RemoveBookById(bookId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task RemoveBookById_ReturnsForbidden_WhenUserIsNotAdminOrCreator()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            var result = await _bookController.RemoveBookById(bookId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}