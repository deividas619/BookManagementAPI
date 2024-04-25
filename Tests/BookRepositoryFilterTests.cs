using Moq;
using BookManagementAPI.Services.Repositories;
using BookManagementAPI;
using BookManagementAPI.Models;
using BookManagementAPI.DTOs;

namespace Tests
{
    public class BookRepositoryFilterTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly BookRepository _bookRepository;

        public BookRepositoryFilterTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _bookRepository = new BookRepository(_mockContext.Object);

            FillDatabase(_mockContext.Object);
        }

        private void FillDatabase(ApplicationDbContext dbContext)
        {
            Genre fantasy = new() { Name = "Fantasy" };
            Genre scifi = new() { Name = "Science Fiction" };
            Genre horror = new() { Name = "Horror" };
            Genre romance = new() { Name = "Romance" };

            dbContext.Genres.AddRange(fantasy, scifi, horror, romance);

            List<Book> books = new()
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
                {                    Author = "J.K. Rowling",
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

            dbContext.Books.AddRange(books);
            dbContext.SaveChanges();
        }

        
    }
}
