using BookManagementAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BookManagementAPI.DTOs
{
    public class ReviewDto
    {
        public string Text { get; set; }
        public int Rating { get; set; }
        public string BookTitle { get; set; } = string.Empty;

    }
}
