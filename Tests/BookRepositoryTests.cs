using Microsoft.EntityFrameworkCore;
using Moq;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;
using BookManagementAPI;

namespace Tests
{
    public class BookRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private BookRepository _bookRepository;

        //public BookRepositoryTests()
        //{
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
        //        .Options;
        //    _mockContext = new Mock<ApplicationDbContext>(options);
        //    _bookRepository = new BookRepository(_mockContext.Object);
        //}

        //[Fact]
        //public async Task GetAllBooks_ReturnsListOfBooks_WhenBooksExist()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books).ReturnsDbSet(new List<Book> { new Book(), new Book() });

        //    // Act
        //    var result = await _bookRepository.GetAllBooks();

        //    // Assert
        //    Assert.Equal(2, result.Count());
        //}

        //[Fact]
        //public async Task GetAllBooks_ThrowsException_WhenFailedToGetBooks()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books).Throws(new Exception());

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _bookRepository.GetAllBooks());
        //}

        //[Fact]
        //public async Task GetBooksByTitle_ReturnsListOfBooks_WhenBooksExist()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books).ReturnsDbSet(new List<Book> { new Book(), new Book() });

        //    // Act
        //    var result = await _bookRepository.GetBooksByTitle("Title");

        //    // Assert
        //    Assert.Equal(2, result.Count());
        //}

        //[Fact]
        //public async Task GetBooksByTitle_ThrowsException_WhenFailedToGetBooks()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books).Throws(new Exception());

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _bookRepository.GetBooksByTitle("Title"));
        //}

        //[Fact]
        //public async Task GetBookById_ReturnsBook_WhenBookExists()
        //{
        //    // Arrange
        //    var book = new Book();
        //    _mockContext.Setup(c => c.Books.FindAsync(It.IsAny<Guid>())).ReturnsAsync(book);

        //    // Act
        //    var result = await _bookRepository.GetBookById(Guid.NewGuid());

        //    // Assert
        //    Assert.Equal(book, result);
        //}

        //[Fact]
        //public async Task GetBookById_ThrowsException_WhenFailedToGetBook()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books.FindAsync(It.IsAny<Guid>())).Throws(new Exception());

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _bookRepository.GetBookById(Guid.NewGuid()));
        //}

        //[Fact]
        //public async Task AddBook_ReturnsBook_WhenAdditionIsSuccessful()
        //{
        //    // Arrange
        //    var book = new Book();
        //    _mockContext.Setup(c => c.Books.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

        //    // Act
        //    var result = await _bookRepository.AddBook(book);

        //    // Assert
        //    Assert.Equal(book, result);
        //}

        //[Fact]
        //public async Task AddBook_ThrowsException_WhenAdditionFails()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>())).Throws(new Exception());

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _bookRepository.AddBook(new Book()));
        //}

        //[Fact]
        //public async Task UpdateBook_ReturnsBook_WhenUpdateIsSuccessful()
        //{
        //    // Arrange
        //    var book = new Book();
        //    _mockContext.Setup(c => c.Books.Update(It.IsAny<Book>())).Returns(book);

        //    // Act
        //    var result = await _bookRepository.UpdateBook(book);

        //    // Assert
        //    Assert.Equal(book, result);
        //}

        //[Fact]
        //public async Task UpdateBook_ThrowsException_WhenUpdateFails()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books.Update(It.IsAny<Book>())).Throws(new Exception());

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _bookRepository.UpdateBook(new Book()));
        //}

        //[Fact]
        //public async Task RemoveBookById_ReturnsBook_WhenRemovalIsSuccessful()
        //{
        //    // Arrange
        //    var book = new Book();
        //    _mockContext.Setup(c => c.Books.FindAsync(It.IsAny<Guid>())).ReturnsAsync(book);

        //    // Act
        //    var result = await _bookRepository.RemoveBookById(Guid.NewGuid());

        //    // Assert
        //    Assert.Equal(book, result);
        //}

        //[Fact]
        //public async Task RemoveBookById_ThrowsException_WhenRemovalFails()
        //{
        //    // Arrange
        //    _mockContext.Setup(c => c.Books.FindAsync(It.IsAny<Guid>())).Throws(new Exception());

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _bookRepository.RemoveBookById(Guid.NewGuid()));
        //}
    }
}
