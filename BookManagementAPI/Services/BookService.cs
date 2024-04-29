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

    public async Task<Book> AddBook(string title, string author, DateOnly publication, GenreDto genreDto, string userName)
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
                return null;
            }
        }
        else
        {
            return currentBook;
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