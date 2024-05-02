using BookManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BookManagementAPI.Interfaces
{
    public interface IBookSuggestionService
    {
        Task<List<Book>> GetBookSuggestionsAsync(Guid userId);
    }
}
