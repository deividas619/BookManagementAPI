using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Book>> GetBooksByFilter(SearchFilterDto filter, int skip, int take);
        Task<Book> GetBookById(Guid id);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book currentBook);
        Task<Book> RemoveBookById(Guid id);
        Task<Genre> GetGenre(GenreDto genre);
        Guid GetUserId(string username);

    }
}