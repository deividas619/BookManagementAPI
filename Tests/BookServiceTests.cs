using BookManagementAPI.DTOs;
using Moq;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;
using BookManagementAPI.Services;

namespace Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockBookRepository.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsListOfBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book> { new Book(), new Book() };
            _mockBookRepository.Setup(r => r.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllBooks();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllBooks_ThrowsException_WhenFailedToGetBooks()
        {
            // Arrange
            _mockBookRepository.Setup(r => r.GetAllBooks()).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.GetAllBooks());
        }

        [Fact]
        public async Task GetBooksByTitle_ReturnsListOfBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book> { new Book(), new Book() };
            _mockBookRepository.Setup(r => r.GetBooksByTitle(It.IsAny<string>())).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetBooksByTitle("Title");

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetBooksByTitle_ThrowsException_WhenFailedToGetBooks()
        {
            // Arrange
            _mockBookRepository.Setup(r => r.GetBooksByTitle(It.IsAny<string>())).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.GetBooksByTitle("Title"));
        }

        [Fact]
        public async Task GetBookById_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var book = new Book();
            _mockBookRepository.Setup(r => r.GetBookById(It.IsAny<Guid>())).ReturnsAsync(book);

            // Act
            var result = await _bookService.GetBookById(Guid.NewGuid());

            // Assert
            Assert.Equal(book, result);
        }

        [Fact]
        public async Task GetBookById_ThrowsException_WhenFailedToGetBook()
        {
            // Arrange
            _mockBookRepository.Setup(r => r.GetBookById(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.GetBookById(Guid.NewGuid()));
        }

        /*[Fact]
        public async Task AddBook_ReturnsBook_WhenAdditionIsSuccessful()
        {
            // Arrange
            var book = new Book();
            _mockBookRepository.Setup(r => r.AddBook(It.IsAny<Book>())).ReturnsAsync(book);

            // Act
            var result = await _bookService.AddBook("Title", "Author", DateOnly.MinValue, new GenreDto());

            // Assert
            Assert.Equal(book, result);
        }*/

        /*[Fact]
        public async Task AddBook_ThrowsException_WhenAdditionFails()
        {
            // Arrange
            _mockBookRepository.Setup(r => r.AddBook(It.IsAny<Book>())).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.AddBook("Title", "Author", DateOnly.MinValue, new GenreDto()));
        }*/

        /*[Fact]
        public async Task UpdateBook_ReturnsBook_WhenUpdateIsSuccessful()
        {
            // Arrange
            var book = new Book();
            _mockBookRepository.Setup(r => r.UpdateBook(It.IsAny<Book>())).ReturnsAsync(book);

            // Act
            var result = await _bookService.UpdateBook(new Book());

            // Assert
            Assert.Equal(book, result);
        }*/

        /*[Fact]
        public async Task UpdateBook_ThrowsException_WhenUpdateFails()
        {
            // Arrange
            _mockBookRepository.Setup(r => r.UpdateBook(It.IsAny<Book>())).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.UpdateBook(new Book()));
        }*/

        [Fact]
        public async Task RemoveBookById_ReturnsBook_WhenRemovalIsSuccessful()
        {
            // Arrange
            var book = new Book();
            _mockBookRepository.Setup(r => r.RemoveBookById(It.IsAny<Guid>())).ReturnsAsync(book);

            // Act
            var result = await _bookService.RemoveBookById(Guid.NewGuid());

            // Assert
            Assert.Equal(book, result);
        }

        [Fact]
        public async Task RemoveBookById_ThrowsException_WhenRemovalFails()
        {
            // Arrange
            _mockBookRepository.Setup(r => r.RemoveBookById(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.RemoveBookById(Guid.NewGuid()));
        }
    }
}
