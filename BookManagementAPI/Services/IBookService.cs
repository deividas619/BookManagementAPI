using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Book>> GetBooksByTitle(string title);
        Task<Book> GetBookById(Guid id);
        Task<Book> AddBook(string title, string author, DateOnly publication, Genre genre);
        Task<Book> UpdateBook(Book currentBook);
        Task<Book> RemoveBookById(Guid id);
    }
}
