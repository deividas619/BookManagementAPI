using Microsoft.EntityFrameworkCore;
using Moq;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;
using BookManagementAPI;

namespace Tests
{
    public class BookRepositoryTests
    {
        [Fact]
        public void GetBook_ExistingTitle_ReturnsBook()
        {
            // Arrange
            var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "Book 1" },
            new Book { Id = Guid.NewGuid(), Title = "Book 2" }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Book>>();
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Books).Returns(mockDbSet.Object);

            var bookRepository = new BookRepository(mockContext.Object);

            // Act
            var result = bookRepository.GetBook("Book 1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Book 1", result.Title);
        }

        [Fact]
        public void AddBook_ValidBook_AddsToContext()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Books).Returns(mockDbSet.Object);

            var bookRepository = new BookRepository(mockContext.Object);
            var book = new Book { Id = Guid.NewGuid(), Title = "New Book" };

            // Act
            bookRepository.AddBook(book);

            // Assert
            mockDbSet.Verify(m => m.Add(book), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void RemoveBook_ExistingId_RemovesFromContext()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId, Title = "Book to Remove" };
            var data = new List<Book> { book }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Book>>();
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockDbSet.Setup(m => m.Remove(It.IsAny<Book>())).Verifiable();

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Books).Returns(mockDbSet.Object);

            var bookRepository = new BookRepository(mockContext.Object);

            // Act
            bookRepository.RemoveBook(bookId);

            // Assert
            mockDbSet.Verify(m => m.Remove(book), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
