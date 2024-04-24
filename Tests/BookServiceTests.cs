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
        public void AddBook_NewBook_SuccessfullyAdded()
        {
            // Arrange
            var newBook = new Book { Title = "New Book" };
            _mockBookRepository.Setup(repo => repo.GetBook(newBook.Title)).Returns((Book)null);

            // Act
            var response = _bookService.AddBook(newBook);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Empty(response.Message);
            _mockBookRepository.Verify(repo => repo.AddBook(newBook), Times.Once);
        }

        [Fact]
        public void AddBook_ExistingBook_FailsToAdd()
        {
            // Arrange
            var existingBook = new Book { Title = "Existing Book" };
            _mockBookRepository.Setup(repo => repo.GetBook(existingBook.Title)).Returns(existingBook);

            // Act
            var response = _bookService.AddBook(existingBook);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal("Book already exist", response.Message);
            _mockBookRepository.Verify(repo => repo.AddBook(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public void GetBook_ExistingTitle_ReturnsBook()
        {
            // Arrange
            var existingBook = new Book { Title = "Existing Book" };
            _mockBookRepository.Setup(repo => repo.GetBook(existingBook.Title)).Returns(existingBook);

            // Act
            var result = _bookService.GetBook(existingBook.Title);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingBook, result);
        }

        [Fact]
        public void GetBook_NonExistingTitle_ReturnsNull()
        {
            // Arrange
            var nonExistingTitle = "Non Existing Book";
            _mockBookRepository.Setup(repo => repo.GetBook(nonExistingTitle)).Returns((Book)null);

            // Act
            var result = _bookService.GetBook(nonExistingTitle);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void RemoveBook_ExistingId_RemovesBook()
        {
            // Arrange
            var existingId = new Guid("b1b9b58a-7e10-4fcb-88a1-5f28b18cc1e6");

            // Act
            _bookService.RemoveBook(existingId);

            // Assert
            _mockBookRepository.Verify(repo => repo.RemoveBook(existingId), Times.Once);
        }
    }
}
