using BookManagementAPI.DTOs;
using Moq;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using BookManagementAPI.Interfaces;

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
        public async Task GetAllBooks_ReturnsAllBooksFromRepository()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1" },
                new Book { Id = Guid.NewGuid(), Title = "Book 2" },
                new Book { Id = Guid.NewGuid(), Title = "Book 3" }
            };
            _mockBookRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllBooks();

            // Assert
            Assert.Equal(3, ((List<Book>)result).Count);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsEmptyList_WhenNoBooksInRepository()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(new List<Book>());

            // Act
            var result = await _bookService.GetAllBooks();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBooksByFilter_ReturnsEmptyList_WhenNoBooksMatchFilter()
        {
            // Arrange
            var filter = new SearchFilterDto { Title = "Non-existing Title", Author = "Non-existing Author", Genres = new string[] { "Non-existing Genre" } };
            _mockBookRepository.Setup(repo => repo.GetBooksByFilter(filter, 0, 0)).ReturnsAsync(new List<Book>());

            // Act
            var result = await _bookService.GetBooksByFilter(filter, 0, 0);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddBook_ReturnsAddedBook()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "New Book" };
            _mockBookRepository.Setup(repo => repo.AddBook(It.IsAny<Book>())).ReturnsAsync(book);

            // Act
            var result = await _bookService.AddBook("New Book", "Author", new DateOnly(), new GenreDto { Name = "Genre" }, "UserName");

            // Assert
            Assert.Equal("New Book", result.Title);
        }

        [Fact]
        public async Task AddBook_ReturnsNull_WhenTitleIsNull()
        {
            // Arrange
            var bookDto = new BookDto { Title = null, Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };

            // Act
            var result = await _bookService.AddBook(bookDto.Title, bookDto.Author, bookDto.Publication, bookDto.Genre, "UserName");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddBook_ReturnsNull_WhenRepositoryFailsToAddBook()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetGenre(It.IsAny<GenreDto>())).ReturnsAsync(new Genre());
            _mockBookRepository.Setup(repo => repo.AddBook(It.IsAny<Book>())).ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.AddBook("New Book", "Author", new DateOnly(), new GenreDto { Name = "Genre" }, "UserName");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddBook_ReturnsNull_WhenPublicationDateIsInFuture()
        {
            // Arrange
            var futureDate = new DateOnly(DateTime.Now.Year + 1, 1, 1);
            var bookDto = new BookDto { Title = "Future Book", Author = "Author", Publication = futureDate, Genre = new GenreDto { Name = "Genre" } };

            // Act
            var result = await _bookService.AddBook(bookDto.Title, bookDto.Author, bookDto.Publication, bookDto.Genre, "UserName");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGenre_ReturnsExistingGenreFromRepository()
        {
            // Arrange
            var genreDto = new GenreDto { Name = "ExistingGenre" };
            var existingGenre = new Genre { Id = Guid.NewGuid(), Name = "ExistingGenre" };
            _mockBookRepository.Setup(repo => repo.GetGenre(genreDto)).ReturnsAsync(existingGenre);

            // Act
            var result = await _bookService.GetGenre(genreDto);

            // Assert
            Assert.Equal(existingGenre, result);
        }

        [Fact]
        public async Task UpdateBook_ReturnsBookWithNotFoundTitle_WhenBookNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Mocking the repository to return a Book with title "Not found" when GetBookById is called with the specified bookId
            _mockBookRepository.Setup(repo => repo.GetBookById(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.UpdateBook(bookId, new BookDto(), "UserName", "Admin");

            // Assert
            // Asserting that the result is not null and that its title is "Not found"
            Assert.NotNull(result);
            Assert.Equal("Not found", result.Title);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNull_WhenRepositoryFailsToUpdateBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDto = new BookDto { Title = "Updated Book", Author = "Author", Publication = new DateOnly(), Genre = new GenreDto { Name = "Genre" } };
            _mockBookRepository.Setup(repo => repo.GetBookById(bookId)).ReturnsAsync(new Book { Id = bookId });
            _mockBookRepository.Setup(repo => repo.UpdateBook(It.IsAny<Book>())).ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.UpdateBook(bookId, bookDto, "UserName", "Admin");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateBook_ReturnsBookWithUnauthorizedTitle_WhenUserIsNotAdminOrCreator()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId, CreatedByUserId = Guid.NewGuid() };
            _mockBookRepository.Setup(repo => repo.GetBookById(bookId)).ReturnsAsync(book);

            // Act
            var result = await _bookService.UpdateBook(bookId, new BookDto(), "NonCreatorUserName", "NonAdminRole");

            // Assert
            // Asserting that the result is not null and that its title is "Unauthorized"
            Assert.NotNull(result);
            Assert.Equal("Unauthorized", result.Title);
        }

        [Fact]
        public async Task RemoveBookById_ReturnsBookWithNotFoundTitle_WhenBookNotFound()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Mocking the repository to return a Book with title "Not found" when RemoveBookById is called with the specified bookId
            _mockBookRepository.Setup(repo => repo.RemoveBookById(bookId)).ReturnsAsync(new Book { Title = "Not found" });

            // Act
            var result = await _bookService.RemoveBookById(bookId, "test", "test");

            // Assert
            // Asserting that the result is not null and that its title is "Not found"
            Assert.NotNull(result);
            Assert.Equal("Not found", result.Title);
        }

        /*[Fact]
        public async Task RemoveBookById_ReturnsBook_WhenRemovalIsSuccessful()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookToRemove = new Book { Id = bookId, Title = "Book to Remove", Author = "Author", Publication = new DateOnly(), Genre = new Genre { Name = "Genre" } };
            _mockBookRepository.Setup(repo => repo.RemoveBookById(bookId)).ReturnsAsync(bookToRemove);

            // Act
            var result = await _bookService.RemoveBookById(bookId, "test", "test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
            Assert.Equal("Book to Remove", result.Title);
        }*/
    }
}