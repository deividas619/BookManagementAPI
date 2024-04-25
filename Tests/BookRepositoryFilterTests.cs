using Moq;
using BookManagementAPI.Services.Repositories;
using BookManagementAPI;
using BookManagementAPI.Models;
using BookManagementAPI.DTOs;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class BookRepositoryFilterTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BookRepository _bookRepository;
        private ImmutableList<Book> _books = default!;

        public BookRepositoryFilterTests()
        {
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
			_dbContext = new ApplicationDbContext(options);
			_bookRepository = new BookRepository(_dbContext);

            FillDatabase(_dbContext);
        }

        private void FillDatabase(ApplicationDbContext dbContext)
        {
            Genre fantasy = new() { Name = "Fantasy" };
            Genre scifi = new() { Name = "Science Fiction" };
            Genre horror = new() { Name = "Horror" };
            Genre romance = new() { Name = "Romance" };

            dbContext.Genres.AddRange(fantasy, scifi, horror, romance);

            var books = new Book[]
            {
                new()
                {
                    Author = "author1",
                    Title = "book1",
                    Publication = new DateOnly(2021, 10, 11),
                    Genre = fantasy
                },
                new()
                {
                    Author = "author2",
                    Title = "book2",
                    Publication = new DateOnly(2024, 8, 21),
                    Genre = fantasy
                },
                new()
                {
                    Author = "author3",
                    Title = "book3",
                    Publication = new DateOnly(2018, 9, 5),
                    Genre = horror
                },
                new()
                {
                    Author = "George Orwell",
                    Title = "1984",
                    Publication = new DateOnly(1949, 6, 8),
                    Genre = scifi
                },
                new()
                {
                    Author = "Tolkien",
                    Title = "The Lord of the Rings",
                    Publication = new DateOnly(1954, 7, 29),
                    Genre = fantasy
                },
                new()
                {   
                    Author = "J.K. Rowling",
                    Title = "Harry Potter",
                    Publication = new DateOnly(1997, 6, 26),
                    Genre = fantasy
                },
                new()
                {
                    Author = "Jane Austen",
                    Title = "Pride and Prejudice",
                    Publication = new DateOnly(1813, 1, 28),
                    Genre = romance
                },
                new()
                {
					Author = "Emily Bronte",
					Title = "Wuthering Heights",
					Publication = new DateOnly(1847, 12, 19),
					Genre = romance
				}
            };

            _books = books.ToImmutableList();

            dbContext.Books.AddRange(_books);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetBooksByEmptyFilter_ReturnsListOfBooks_WhenBooksExist()
        { 
            // Arrange
            SearchFilterDto filter = new()
            {
                Author = null,
                Title = null,
                Genres = Array.Empty<string>(),
                PublicationAfterDate = null,
                PublicationBeforeDate = null
            };

            int skip = 0;
            int take = 5;

			// Act
			var result = await _bookRepository.GetBooksByFilter(filter, skip, take);

			// Assert
			Assert.Equal(take, result.Count());
            Assert.True(result.Select(b => b.Title).SequenceEqual(_books.Skip(skip).Take(take).Select(b => b.Title)));
		}

        [Fact]
        public async Task GetBooksEmptyFilterSkip5Take10_ReturnsListOfBooks_WhenBooksExist()
        {
			// Arrange
			SearchFilterDto filter = new()
            {
				Author = null,
                Title = null,
                Genres = Array.Empty<string>(),
                PublicationAfterDate = null,
                PublicationBeforeDate = null
            };

            int skip = 5;
            int take = 10;

            // Act
            var result = await _bookRepository.GetBooksByFilter(filter, skip, take);
            var expectedBooks = _books.Select(b => b.Title);

            // Assert
            Assert.All(result, b => Assert.Contains(b.Title, expectedBooks));
		}

        [Theory]
        [InlineData("book1")]
        [InlineData("book2")]
        [InlineData("o")]
        [InlineData("O")]
        [InlineData("1984")]
        [InlineData("Harry Potter")]
		public async Task GetBooksByTitleFilter_ReturnsListOfBooks_WhenBooksExist(string title)
        {
            // Arrange
			SearchFilterDto filter = new()
            {
				Author = null,
				Title = title,
				Genres = Array.Empty<string>(),
				PublicationAfterDate = null,
				PublicationBeforeDate = null
			};

			int skip = 0;
			int take = 5;

			// Act
			var result = await _bookRepository.GetBooksByFilter(filter, skip, take);

			// Assert
			Assert.All(result, b => Assert.Contains(title, b.Title, StringComparison.InvariantCultureIgnoreCase));
        }

        [Theory]
        [InlineData("author1")]
        [InlineData("author2")]
        [InlineData("author3")]
        [InlineData("George Orwell")]
        [InlineData("Tolkien")]
        public async Task GetBooksByAuthorFilter_ReturnsListOfBooks_WhenBooksExist(string author)
        {
			// Arrange
			SearchFilterDto filter = new()
            {
				Author = author,
				Title = null,
				Genres = Array.Empty<string>(),
				PublicationAfterDate = null,
				PublicationBeforeDate = null
			};

			int skip = 0;
			int take = 5;

			// Act
			var result = await _bookRepository.GetBooksByFilter(filter, skip, take);

			// Assert
			Assert.All(result, b => Assert.Contains(author, b.Author, StringComparison.InvariantCultureIgnoreCase));
		}

        [Theory]
        [InlineData("Fantasy")]
        [InlineData("Science Fiction")]
        [InlineData("Horror")]
        [InlineData("Romance")]
        public async Task GetBooksByGenreFilter_ReturnsListOfBooks_WhenBooksExist(string genre)
        {
            // Arrange
            SearchFilterDto filter = new()
            {
                Author = null,
                Title = null,
                Genres = new string[] { genre },
                PublicationAfterDate = null,
                PublicationBeforeDate = null
            };

            int skip = 0;
            int take = 5;

            // Act
            var result = await _bookRepository.GetBooksByFilter(filter, skip, take);
            var expectedBooks = _books.Where(b => b.Genre.Name == genre).Take(take).Select(b => b.Title);

            // Assert
            Assert.All(result, b => Assert.Contains(b.Title, expectedBooks));
        }

        [Theory]
        [InlineData("Fantasy", "Science Fiction")]
        [InlineData("Horror", "Romance")]
        [InlineData("Fantasy", "Romance")]
		public async Task GetBooksByGenresFilter_ReturnsListOfBooks_WhenBooksExist(string genre1, string genre2)
        {
			// Arrange
			SearchFilterDto filter = new()
            {
				Author = null,
				Title = null,
				Genres = new string[] { genre1, genre2 },
				PublicationAfterDate = null,
				PublicationBeforeDate = null
			};

			int skip = 0;
			int take = 5;

			// Act
			var result = await _bookRepository.GetBooksByFilter(filter, skip, take);
			var expectedBooks = _books.Where(b => b.Genre.Name == genre1 || b.Genre.Name == genre2).Take(take).Select(b => b.Title);

			// Assert
			Assert.All(result, b => Assert.Contains(b.Title, expectedBooks));
		}

        [Theory]
        [InlineData("2021-10-11")]
        [InlineData("2024-08-21")]
        [InlineData("2018-09-05")]
        [InlineData("1949-06-08")]
        public async Task GetBooksByPublicationFromDateFilter_ReturnsListOfBooks_WhenBooksExist(string publicationFromDate)
        {
			// Arrange
			SearchFilterDto filter = new()
            {
				Author = null,
				Title = null,
				Genres = Array.Empty<string>(),
				PublicationAfterDate = DateOnly.Parse(publicationFromDate),
				PublicationBeforeDate = null
			};

			int skip = 0;
			int take = 5;

			// Act
			var result = await _bookRepository.GetBooksByFilter(filter, skip, take);
			var expectedBooks = _books.Where(b => b.Publication >= filter.PublicationAfterDate).Take(take).Select(b => b.Title);

			// Assert
			Assert.All(result, b => Assert.Contains(b.Title, expectedBooks));
		}

        [Theory]
        [InlineData("2021-10-11")]
        [InlineData("2024-08-21")]
        [InlineData("2018-09-05")]
        [InlineData("1949-06-08")]
		public async Task GetBooksByPublicationToDateFilter_ReturnsListOfBooks_WhenBooksExist(string publicationToDate)
        {
			// Arrange
			SearchFilterDto filter = new()
            {
				Author = null,
				Title = null,
				Genres = Array.Empty<string>(),
				PublicationAfterDate = null,
				PublicationBeforeDate = DateOnly.Parse(publicationToDate)
			};

			int skip = 0;
			int take = 5;

			// Act
			var result = await _bookRepository.GetBooksByFilter(filter, skip, take);
			var expectedBooks = _books.Where(b => b.Publication <= filter.PublicationBeforeDate).Take(take).Select(b => b.Title);

			// Assert
			Assert.All(result, b => Assert.Contains(b.Title, expectedBooks));
		}

        [Theory]
        [InlineData("2021-10-11", "2024-08-21")]
        [InlineData("2018-09-05", "1949-06-08")]
        [InlineData("2021-10-11", "1949-06-08")]
        public async Task GetBooksByPublicationFromToDateFilter_ReturnsListOfBooks_WhenBooksExist(string publicationFromDate, string publicationToDate)
        {
            // Arrange
            SearchFilterDto filter = new()
            {
				Author = null,
				Title = null,
				Genres = Array.Empty<string>(),
				PublicationAfterDate = DateOnly.Parse(publicationFromDate),
				PublicationBeforeDate = DateOnly.Parse(publicationToDate)
			};

            int skip = 0;
            int take = 5;

            // Act
            var result = await _bookRepository.GetBooksByFilter(filter, skip, take);
            var expectedBooks = _books.Where(b => b.Publication >= filter.PublicationAfterDate && b.Publication <= filter.PublicationBeforeDate).Take(take).Select(b => b.Title);

            // Assert
            Assert.All(result, b => Assert.Contains(b.Title, expectedBooks));
        }

        [Theory]
        [InlineData("book1", "author1", "Fantasy", "2021-10-11", "2021-10-11")]
        [InlineData("book2", "author2", "Fantasy", "2024-08-21", "2024-08-21")]
        [InlineData("book3", "author3", "Horror", "2018-09-05", "2018-09-05")]
        [InlineData("1984", "George Orwell", "Science Fiction", "1949-06-08", "1949-06-08")]
        public async Task GetBooksByAllFilters_ReturnsListOfBooks_WhenBooksExist(string title, string author, string genre, string publicationFromDate, string publicationToDate)
        {
            // Arrange
            SearchFilterDto filter = new()
            {
                Author = author,
                Title = title,
                Genres = new string[] { genre },
                PublicationAfterDate = DateOnly.Parse(publicationFromDate),
                PublicationBeforeDate = DateOnly.Parse(publicationToDate)
            };

            int skip = 0;
            int take = 5;

            // Act
            var result = await _bookRepository.GetBooksByFilter(filter, skip, take);
            var firstBook = result.First();

            // Assert
            Assert.Equal(title, firstBook.Title);
            Assert.Equal(author, firstBook.Author);
            Assert.Equal(DateOnly.Parse(publicationFromDate), firstBook.Publication);
            Assert.Equal(DateOnly.Parse(publicationToDate), firstBook.Publication);
        }
	}
}
