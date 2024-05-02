using System;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookSuggestionsController (IBookSuggestionService bookSuggestionService) : ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBookSuggestions(Guid userId)
        {
            var suggestions = await bookSuggestionService.GetBookSuggestionsAsync(userId);
            if (suggestions == null || !suggestions.Any())
                return NotFound();

            return Ok(suggestions);
        }
    }
}
