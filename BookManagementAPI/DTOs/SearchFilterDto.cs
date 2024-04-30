using System;

namespace BookManagementAPI.DTOs
{
    public class SearchFilterDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string[] Genres { get; set; }
        public DateOnly? PublicationAfterDate { get; set; }
        public DateOnly? PublicationBeforeDate { get; set; }
    }
}
