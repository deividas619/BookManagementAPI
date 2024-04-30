//using Microsoft.EntityFrameworkCore;
//using Moq;
//using BookManagementAPI.Models;
//using BookManagementAPI.Services.Repositories;
//using BookManagementAPI;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Newtonsoft.Json;
//using System.Diagnostics;

//namespace Tests
//{
//    public class BookRepositoryTests
//    {
//        private Mock<DbSet<Book>> _mockDbSet;
//        private readonly Mock<ApplicationDbContext> _mockContext;
//        private readonly BookRepository _bookRepository;

//        public BookRepositoryTests()
//        {
//            _mockContext = new Mock<ApplicationDbContext>();
//            _bookRepository = new BookRepository(_mockContext.Object);
//        }

        private void SetupMockDbSet(IEnumerable<Book> books)
        {
            _mockDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.AsQueryable().Provider);
            _mockDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.AsQueryable().Expression);
            _mockDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.AsQueryable().ElementType);
            _mockDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(books.AsQueryable().GetEnumerator());

            _mockDbSet.As<IAsyncEnumerable<Book>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(books.ToAsyncEnumerable().GetAsyncEnumerator());
        }

        [Fact]
        public async Task GetAllBooks_ReturnsListOfBooks_WhenBooksExist()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Author = "author1", Title = "book1", Publication = new DateOnly(2024, 1, 1), Genre = new Genre() },
                new Book { Id = Guid.NewGuid(), Author = "author2", Title = "book2", Publication = new DateOnly(2024, 1, 1), Genre = new Genre() }
            };

//            _mockDbSet = new Mock<DbSet<Book>>();
//            SetupMockDbSet(books);

//            _mockContext.Setup(c => c.Books).Returns(_mockDbSet.Object);

//            // Act
//            var result = await _bookRepository.GetAllBooks();

//            // Assert
//            Assert.Equal(2, result.Count());
//        }

//        [Fact]
//        public async Task GetAllBooks_ThrowsException_WhenFailedToGetBooks()
//        {
//            // Arrange
//            _mockContext.Setup(c => c.Books).Throws(new Exception());

//            // Act & Assert
//            await Assert.ThrowsAsync<Exception>(() => _bookRepository.GetAllBooks());
//        }

        [Fact]
        public async Task GetBookById_ThrowsException_WhenFailedToGetBook()
        {
            // Arrange
            _mockContext.Setup(c => c.Books).Throws(new Exception());

//            // Act & Assert
//            await Assert.ThrowsAsync<Exception>(() => _bookRepository.GetBookById(Guid.NewGuid()));
//        }

        [Fact]
        public async Task AddBook_ThrowsException_WhenAdditionFails()
        {
            // Arrange
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _bookRepository.AddBook(new Book()));
        }

        [Fact]
        public async Task UpdateBook_ReturnsBook_WhenUpdateIsSuccessful()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Author = "author2", Title = "book2", Publication = new DateOnly(2024, 1, 1), Genre = new Genre() };
            _mockContext.Setup(c => c.Books.Update(book)).Returns((EntityEntry<Book>)null);

//            // Act
//            var result = await _bookRepository.UpdateBook(book);

//            // Debug
//            Debug.WriteLine("Expected Book:");
//            Debug.WriteLine(JsonConvert.SerializeObject(book));
//            Debug.WriteLine("Actual Book:");
//            Debug.WriteLine(JsonConvert.SerializeObject(result));

            // Assert
            Assert.Equal(book, result);
        }

//        [Fact]
//        public async Task RemoveBookById_ReturnsBook_WhenRemovalIsSuccessful()
//        {
//            // Arrange
//            var book = new Book();
//            _mockContext.Setup(c => c.Books.FindAsync(It.IsAny<Guid>())).ReturnsAsync(book);

//            // Act
//            var result = await _bookRepository.RemoveBookById(Guid.NewGuid());

//            // Assert
//            Assert.Equal(book, result);
//        }

//        [Fact]
//        public async Task RemoveBookById_ThrowsException_WhenRemovalFails()
//        {
//            // Arrange
//            _mockContext.Setup(c => c.Books.FindAsync(It.IsAny<Guid>())).Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookRepository.RemoveBookById(Guid.NewGuid()));
        }
    }
}
