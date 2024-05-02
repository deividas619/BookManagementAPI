using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooks();
    Task<IEnumerable<Book>> GetBooksByFilter(SearchFilterDto searchFilter, int skip, int take);
    Task<IEnumerable<Book>> GetBookSuggestions(Guid bookId);
    Task<Book> AddBook(string title, string author, DateOnly publication, GenreDto genre, string userName);
    Task<Book> UpdateBook(Guid currentBookId, BookDto currentBook, string userName, string userNameRole);
    Task<Book> RemoveBookById(Guid id, string userName, string userNameRole);
}