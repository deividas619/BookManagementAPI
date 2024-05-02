using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Book>> GetBooksByFilter(SearchFilterDto filter, int skip, int take);
        Task<IEnumerable<Book>> GetBookSuggestions([FromRoute] Guid bookId);
        Task<Book> GetBookById(Guid id);
        IEnumerable<Book> GetBooksByGenre(Guid genreId);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book currentBook);
        Task<Book> RemoveBookById(Guid id);
        Task<Genre> GetGenre(GenreDto genre);
        Guid GetUserId(string username);
    }
}