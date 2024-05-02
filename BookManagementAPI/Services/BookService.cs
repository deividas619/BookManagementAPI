using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.IdentityModel.Tokens;
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

    public async Task<IEnumerable<Book>> GetBooksByFilter(SearchFilterDto searchFilter, int skip, int take)
    {
        try
        {
            return await repository.GetBooksByFilter(searchFilter, skip, take);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBooksByFilter)}]: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<Book>> GetBookSuggestions(Guid bookId)
    {
        try
        {
            return await repository.GetBookSuggestions(bookId);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetBookSuggestions)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> AddBook(string title, string author, DateOnly publication, GenreDto genreDto, string userName)
    {
        if (title.IsNullOrEmpty() || author.IsNullOrEmpty() || genreDto.Name.IsNullOrEmpty())
        {
            return null;
        }
        else
        {
            try
            {
                var genre = await GetGenre(genreDto);
                return await repository.AddBook(new Book
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Author = author,
                    Publication = publication,
                    Genre = genre,
                    CreatedByUserId = repository.GetUserId(userName)
                });
            }
            catch (Exception e)
            {
                Log.Error($"[{nameof(AddBook)}]: {e.Message}");
                throw;
            }
        }
    }

    public async Task<Genre> GetGenre(GenreDto genre)
    {
        try
        {
            return await repository.GetGenre(genre);
        }
        catch (Exception e)
        {
            Log.Error($"[{nameof(GetGenre)}]: {e.Message}");
            throw;
        }
    }

    public async Task<Book> UpdateBook(Guid bookId, BookDto currentBookDto, string userName, string userNameRole)
    {
        var currentBook = await repository.GetBookById(bookId);
        if (currentBook is not null)
        {
            if (repository.GetUserId(userName) == currentBook.CreatedByUserId || string.Compare(userNameRole, "Admin", StringComparison.OrdinalIgnoreCase) == 0)
            {
                try
                {
                    currentBook.Title = currentBookDto.Title;
                    currentBook.Author = currentBookDto.Author;
                    currentBook.Publication = currentBookDto.Publication;
                    currentBook.Genre = await GetGenre(currentBookDto.Genre);

                    return await repository.UpdateBook(currentBook);
                }
                catch (Exception e)
                {
                    Log.Error($"[{nameof(UpdateBook)}]: {e.Message}");
                    throw;
                }
            }
            else
            {
                Log.Error($"[{nameof(UpdateBook)}]: User is not authorized to edit Book with id: {bookId}!");
                return new Book { Title = "Unauthorized" };
            }
        }
        else
        {
            Log.Error($"[{nameof(UpdateBook)}]: No Book match was found with id: {bookId}!");
            return new Book { Title = "Not found" };
        }
    }

    public async Task<Book> RemoveBookById(Guid bookId, string userName, string userNameRole)
    {
        var currentBook = await repository.GetBookById(bookId);
        if (currentBook is not null)
        {
            if (repository.GetUserId(userName) == currentBook.CreatedByUserId || string.Compare(userNameRole, "Admin", StringComparison.OrdinalIgnoreCase) == 0)
            {
                try
                {
                    return await repository.RemoveBookById(bookId);
                }
                catch (Exception e)
                {
                    Log.Error($"[{nameof(RemoveBookById)}]: {e.Message}");
                    throw;
                }
            }
            else
            {
                Log.Error($"[{nameof(RemoveBookById)}]: User is not authorized to delete Book with id: {bookId}!");
                return new Book { Title = "Unauthorized" };
            }
        }
        else
        {
            Log.Error($"[{nameof(RemoveBookById)}]: No Book match was found with id: {bookId}!");
            return new Book { Title = "Not found" };
        }
    }
}