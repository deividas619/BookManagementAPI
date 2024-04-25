using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;
using Serilog;

namespace BookManagementAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository repository)
        {
            _bookRepository = repository;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            try
            {
                return await _bookRepository.GetAllBooks();
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(GetAllBooks)}]: {e.Message}");
                throw;
            }
            
        }

        public async Task<IEnumerable<Book>> GetBooksByTitle(string title)
        {
            try
            {
                return await _bookRepository.GetBooksByTitle(title);
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(GetBooksByTitle)}]: {e.Message}");
                throw;
            }
        }

        public async Task<Book> GetBookById(Guid id)
        {
            try
            {
                return await _bookRepository.GetBookById(id);
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(GetBookById)}]: {e.Message}");
                throw;
            }
        }

        public async Task<Book> AddBook(string title, string author, DateOnly publication, Genre genre)
        {
            try
            {
                return await _bookRepository.AddBook(new Book
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Author = author,
                    Publication = publication,
                    Genre = genre
                });
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(AddBook)}]: {e.Message}");
                throw;
            }
        }

        public async Task<Book> UpdateBook(Book currentBook)
        {
            try
            {
                return await _bookRepository.UpdateBook(currentBook);
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(UpdateBook)}]: {e.Message}");
                throw;
            }
        }

        public async Task<Book> RemoveBookById(Guid id)
        {
            try
            {
                return await _bookRepository.RemoveBookById(id);
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(RemoveBookById)}]: {e.Message}");
                throw;
            }
        }
    }
}
