using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;
using Serilog;

namespace BookManagementAPI.Services;

public class BookService(IBookRepository repository) : IBookService
{
    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        try
        {
            return await repository.GetAllBooks();
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
            return await repository.GetBooksByTitle(title);
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
            return await repository.GetBookById(id);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBookById)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> AddBook(string title, string author, DateOnly publication, GenreDto genre)
    {
        try
        {
            return await repository.AddBook(new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Author = author,
                Publication = publication,
                Genre = new Genre { Id = Guid.NewGuid(), Name = genre.Name }
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
            return await repository.UpdateBook(currentBook);
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
            return await repository.RemoveBookById(id);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(RemoveBookById)}]: {e.Message}");
            throw;
        }
    }
}